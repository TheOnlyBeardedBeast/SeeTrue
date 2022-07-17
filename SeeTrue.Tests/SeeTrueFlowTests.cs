using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SeeTrue.Infrastructure.Commands;
using SeeTrue.Infrastructure.Extensions;
using SeeTrue.Infrastructure.Queries;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;
using SeeTrue.Models;
using Xunit;
using Xunit.Priority;

namespace SeeTrue.Tests
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class UserFlow : IClassFixture<SeeTrueFixture>
    {
        protected readonly SeeTrueFixture fixture;

        public UserFlow(SeeTrueFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact, Priority(0)]
        public async Task SignUpHandlerTest()
        {
            var handler = new SignUp.Handler(fixture.queries, fixture.commands);

            var requestData = new SignUpRequest
            {
                Email = "test@user.com",
                Password = "12345678",
                UserMetaData = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "name","test user" }
                }
            };

            var command = new SignUp.Command(requestData, Env.Audiences[0]);

            var user = await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(user);
            Assert.Null(user.ConfirmedAt);
            Assert.NotNull(user.ConfirmationToken);

            (string email, string token) = fixture.mailer.mail;

            Assert.Equal(email, user.Email);
            Assert.Equal(token, user.ConfirmationToken);
        }

        [Fact, Priority(1)]
        public async Task VerifySignUp()
        {
            var handler = new Verify.Handler(fixture.queries, fixture.commands);

            (string _, string token) = fixture.mailer.mail;

            var command = new Verify.Command("signup", token, null, "Test user agent");

            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(response);
            Assert.NotNull(response.User.ConfirmedAt);
            Assert.Null(response.User.ConfirmationToken);

            fixture.testCache.Set("refreshToken", response.RefreshToken);
        }

        [Fact, Priority(2)]
        public async Task TokenPasswordGrant()
        {
            var handler = new Token.Handler(fixture.queries, fixture.commands);

            var requestData = new TokenRequest { Email = "test@user.com", Password = "12345678", GrantType = "password" };

            var command = new Token.Command(requestData, Env.Audiences[0], "Test user agent");

            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(response);
        }

        [Fact, Priority(2)]
        public async Task TokenRefreshTokenGrant()
        {
            var handler = new Token.Handler(fixture.queries, fixture.commands);

            var token = fixture.testCache.Get<string>("refreshToken");
            var requestData = new TokenRequest { GrantType = "refresh_token", RefreshToken = token };

            var command = new Token.Command(requestData, Env.Audiences[0], "Test user agent");

            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(response);

            fixture.testCache.Set("accessToken", response.AccessToken);
            fixture.testCache.Set("refreshToken", response.RefreshToken);
        }

        [Fact, Priority(3)]
        public async Task User()
        {
            var handler = new GetUser.Handler(fixture.queries);

            var token = fixture.testCache.Get<string>("accessToken");
            var decoded = Jose.JWT.Decode<DecodedJWT>(token, Env.SigningKey.ToByteArray());

            var query = new GetUser.Query(decoded.sub);

            var response = await handler.Handle(query, new System.Threading.CancellationToken());

            Assert.NotNull(response);
            Assert.Equal(decoded.sub, response.Id);
        }

        [Fact, Priority(3)]
        public async Task UserUpdate()
        {
            var handler = new UserUpdate.Handler(fixture.queries, fixture.commands);

            var token = fixture.testCache.Get<string>("accessToken");
            var decoded = Jose.JWT.Decode<DecodedJWT>(token, Env.SigningKey.ToByteArray());

            var metaCommand = new UserUpdate.Command(decoded.sub, null, null, new Dictionary<string, string> { { "name", "testUserUpdated" } });
            var metaResponse = await handler.Handle(metaCommand, new System.Threading.CancellationToken());

            Assert.NotNull(metaResponse);
            Assert.Equal("testUserUpdated", metaResponse.UserMetaData[Env.NameKey]);

            var oldPassword = metaResponse.EncryptedPassword;

            var passwordCommand = new UserUpdate.Command(decoded.sub, "987654321", null, null);
            var passwordResponse = await handler.Handle(passwordCommand, new System.Threading.CancellationToken());

            Assert.NotNull(passwordResponse);
            Assert.NotEqual("987654321", passwordResponse.EncryptedPassword);
            Assert.NotEqual(oldPassword, passwordResponse.EncryptedPassword);

            var emailCommand = new UserUpdate.Command(decoded.sub, null, "test@userUpdated.com", null);
            var emailResponse = await handler.Handle(emailCommand, new System.Threading.CancellationToken());

            Assert.NotNull(emailResponse);
            Assert.Equal("test@userUpdated.com", emailResponse.EmailChange);
            Assert.NotNull(emailResponse.EmailChangeToken);

            fixture.testCache.Set("emailChangeToken", emailResponse.EmailChangeToken);
        }

        [Fact, Priority(4)]
        public async Task ConfirmEmailChange()
        {
            var token = fixture.testCache.Get<string>("emailChangeToken");

            var handler = new ConfirmEmailChange.Handler(fixture.queries, fixture.commands);

            var command = new ConfirmEmailChange.Command(token);

            await handler.Handle(command, new System.Threading.CancellationToken());

            var userHandler = new GetUser.Handler(fixture.queries);

            var accessToken = fixture.testCache.Get<string>("accessToken");
            var decoded = Jose.JWT.Decode<DecodedJWT>(accessToken, Env.SigningKey.ToByteArray());

            var query = new GetUser.Query(decoded.sub);

            var response = await userHandler.Handle(query, new System.Threading.CancellationToken());

            Assert.Equal("test@userUpdated.com", response.Email);
            Assert.Null(response.EmailChangeToken);
            Assert.Null(response.EmailChange);
        }

        [Fact, Priority(5)]
        public async Task Logout()
        {
            var handler = new Logout.Handler(fixture.queries, fixture.commands);

            var token = fixture.testCache.Get<string>("accessToken");
            var decoded = Jose.JWT.Decode<DecodedJWT>(token, Env.SigningKey.ToByteArray());

            var command = new Logout.Command(decoded.lid);

            await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(fixture.cache.Get(decoded.lid.ToString()));
        }

        [Fact, Priority(6)]
        public async Task RequestMagicLink()
        {
            var handler = new RequestMagicLink.Handler(fixture.queries, fixture.commands, fixture.mailer);

            var command = new RequestMagicLink.Command("test@userUpdated.com", Env.Audiences[0]);

            await handler.Handle(command, new System.Threading.CancellationToken());

            (string email, string token) = fixture.mailer.mail;

            Assert.Equal("test@userUpdated.com", email);
            Assert.NotNull(fixture.cache.Get(token));
        }

        [Fact, Priority(7)]
        public async Task ProcessMagicLink()
        {
            (string _, string token) = fixture.mailer.mail;

            var handler = new ProcessMagicLink.Handler(fixture.commands, fixture.queries);

            var command = new ProcessMagicLink.Command(token, "Test user agent");

            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(response);

            fixture.mailer.mail = (null, null);
        }

        [Fact, Priority(8)]
        public async Task Recover()
        {
            var handler = new Recover.Handler(fixture.queries, fixture.commands);

            var command = new Recover.Command("test@userUpdated.com", Env.Audiences[0]);

            await handler.Handle(command, new System.Threading.CancellationToken());

            (string email, string token) = fixture.mailer.mail;

            Assert.Equal("test@userUpdated.com", email);
            Assert.NotNull(token);
        }

        [Fact, Priority(9)]
        public async Task VerifyRevocer()
        {
            var handler = new Verify.Handler(fixture.queries, fixture.commands);

            (string _, string token) = fixture.mailer.mail;

            var command = new Verify.Command("recovery", token, null, "Test user agent");

            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(response);
            Assert.NotNull(response.User.ConfirmedAt);
            Assert.Null(response.User.RecoveryToken);
        }
    }

    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AdminFlow : IClassFixture<SeeTrueFixture>
    {
        protected readonly SeeTrueFixture fixture;

        public AdminFlow(SeeTrueFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact, Priority(0)]
        public async Task CreateUsers()
        {
            var handler = new CreateUser.Handler(fixture.queries, fixture.commands);

            for (var i = 0; i < 20; i++)
            {
                var command = new CreateUser.Command
                {
                    Email = $"test{i}@user.sk",
                    Password = "12345678",
                    Confirm = true
                };

                var response = await handler.Handle(command, new System.Threading.CancellationToken());

                if (i == 1)
                {
                    fixture.testCache.Set("firstUser", response);
                }

                Assert.NotNull(response);
            }

        }

        [Fact, Priority(1)]
        public async Task GetUsers()
        {
            var handler = new GetUsers.Handler(fixture.queries);

            var response = await handler.Handle(new GetUsers.Query(1, 10), new System.Threading.CancellationToken());

            Assert.Equal(20, response.ItemCount);
            Assert.Equal(10, response.Items.Count);

            response = await handler.Handle(new GetUsers.Query(3, 10), new System.Threading.CancellationToken());

            Assert.Equal(20, response.ItemCount);
            Assert.Empty(response.Items);
        }

        [Fact, Priority(1)]
        public async Task GetUser()
        {
            var user = fixture.testCache.Get<User>("firstUser");
            var handler = new GetUser.Handler(fixture.queries);

            var response = await handler.Handle(new GetUser.Query(user.Id), new System.Threading.CancellationToken());

            Assert.Equal(user.Id, response.Id);
        }

        [Fact, Priority(2)]
        public async Task UpdateUser()
        {
            var user = fixture.testCache.Get<User>("firstUser");
            var handler = new AdminUpdateUser.Handler(fixture.queries, fixture.commands);

            var oldEmail = user.Email;

            var updateRequest = new AdminUpdateUserRequest
            {
                Email = "updated" + user.Email,
            };

            var response = await handler.Handle(new AdminUpdateUser.Command(user.Id, updateRequest), new System.Threading.CancellationToken());

            Assert.NotEqual(oldEmail, response.Email);
        }

        [Fact, Priority(3)]
        public async Task DeleteUser()
        {
            var user = fixture.testCache.Get<User>("firstUser");
            var handler = new DeleteUser.Handler(fixture.queries, fixture.commands);
            var command = new DeleteUser.Command(user.Id);

            await handler.Handle(command, new System.Threading.CancellationToken());

            var count = await fixture.db.Users.CountAsync();

            Assert.Equal(19, count);
        }
    }
}
