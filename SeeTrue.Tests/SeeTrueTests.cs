using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeeTrue.Infrastructure.Utils;
using Xunit;
using Xunit.Priority;

namespace SeeTrue.Tests
{
    // dotnet test /p:CollectCoverage=true
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class SeeTrueTests : IClassFixture<SeeTrueFixture>
    {
        protected readonly SeeTrueFixture fixture;

        public SeeTrueTests(SeeTrueFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact, Priority(0)]
        public async Task SignUpNewUser()
        {
            var user = await fixture.commands.SignUpNewUser("test@user.com", "12345678", "testaudience", "email", new Dictionary<string, object>(), true);

            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.Equal("test@user.com", user.Email);
            Assert.NotEqual("12345678", user.EncryptedPassword);
            Assert.NotNull(user.ConfirmedAt);
            Assert.Equal(Env.JwtDefaultGroupName, user.Role);
            Assert.Equal("testaudience", user.Aud);
        }

        [Fact, Priority(1)]
        public async Task FindUserByEmailAndAudience()
        {
            var user = await fixture.queries.FindUserByEmailAndAudience("test@user.com", "testaudience");

            Assert.NotNull(user);
        }

        [Fact, Priority(1)]
        public async Task CheckEmailExists()
        {
            var userExists = await fixture.queries.CheckEmailExists("test@user.com", "testaudience");

            Assert.True(userExists);
        }

        [Fact, Priority(1)]
        public async Task FindUserById()
        {
            var user = await fixture.queries.FindUserByEmailAndAudience("test@user.com", "testaudience");

            var userById = await fixture.queries.FindUserById(user.Id);

            Assert.NotNull(user);
            Assert.NotNull(userById);

            Assert.Equal(user.Id, userById.Id);
        }

        [Fact, Priority(2)]
        public async Task IssueTokens()
        {
            var user = await fixture.queries.FindUserByEmailAndAudience("test@user.com", "testaudience");

            var tokens = await fixture.commands.IssueTokens(user, Guid.NewGuid());

            Assert.NotNull(tokens);
            Assert.NotEmpty(tokens.AccessToken);
            Assert.NotEmpty(tokens.RefreshToken);
        }

        [Fact, Priority(2)]
        public async Task FindRefreshTokenWithUser()
        {
            var user = await fixture.queries.FindUserByEmailAndAudience("test@user.com", "testaudience");

            var tokens = await fixture.commands.IssueTokens(user, Guid.NewGuid());

            var token = await fixture.queries.FindRefreshTokenWithUser(tokens.RefreshToken);

            Assert.NotNull(tokens);
            Assert.NotEmpty(tokens.RefreshToken);
            Assert.Equal(token.Token, tokens.RefreshToken);
            Assert.Equal(token.UserId, user.Id);
        }

        [Fact, Priority(2)]
        public async Task GrantTokenSwap()
        {
            var user = await fixture.queries.FindUserByEmailAndAudience("test@user.com", "testaudience");

            var tokens = await fixture.commands.IssueTokens(user, Guid.NewGuid());

            var token = await fixture.queries.FindRefreshTokenWithUser(tokens.RefreshToken);
            var newTokens = await fixture.commands.GrantTokenSwap(token);
            var oldToken = await fixture.queries.FindRefreshTokenWithUser(tokens.RefreshToken);

            Assert.NotNull(newTokens);
            Assert.NotEmpty(newTokens.RefreshToken);
            Assert.NotEqual(tokens.RefreshToken, newTokens.RefreshToken);
            Assert.NotEqual(tokens.AccessToken, newTokens.AccessToken);
            Assert.Equal(token.UserId, user.Id);
            Assert.True(oldToken.Revoked);
        }

    }
}
