using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SeeTrue.Infrastructure.Commands;
using SeeTrue.Infrastructure.Extensions;
using SeeTrue.Infrastructure.Queries;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;
using Xunit;
using Xunit.Priority;

namespace SeeTrue.Tests
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class SignupFlow : IClassFixture<SeeTrueFixture>
    {
        protected readonly SeeTrueFixture fixture;

        public SignupFlow(SeeTrueFixture fixture)
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
                UserMetaData = new System.Collections.Generic.Dictionary<string, object>
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
            var requestData = new TokenRequest { GrantType = "refresh_token", RefreshToken = token};

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

        [Fact, Priority(4)]
        public async Task Logout()
        {
            var handler = new Logout.Handler(fixture.queries, fixture.commands);

            var token = fixture.testCache.Get<string>("accessToken");
            var decoded = Jose.JWT.Decode<DecodedJWT>(token, Env.SigningKey.ToByteArray());

            var command = new Logout.Command(decoded.lid);

            await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(fixture.cache.Get(decoded.lid.ToString()));
        }
    }
}
