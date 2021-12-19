using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;

namespace SeeTrue.Infrastructure.Commands
{
    public static class ProcessMagicLink
    {
        public record Command(string Token, string UserAgent) : IRequest<UserTokenResponse>;

        public class Handler : IRequestHandler<Command, UserTokenResponse>
        {
            private readonly ICommandService commands;
            private readonly IQueryService queries;

            public Handler(ICommandService commands, IQueryService queries)
            {
                this.commands = commands;
                this.queries = queries;
            }

            public async Task<UserTokenResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                if (this.queries.TryGetValueFromCache(request.Token, out Guid userId))
                {
                    var user = await queries.FindUserById(userId);

                    if (user is null)
                    {
                        throw new SeeTrueException(HttpStatusCode.BadRequest, "User not found");
                    }

                    this.commands.RemoveFromCache(request.Token);
                    var login = await this.commands.StoreLogin(request.UserAgent, userId);
                    var result = await this.commands.IssueTokens(user, login.Id);

                    return new UserTokenResponse
                    {
                        User = user,
                        AccessToken = result.AccessToken,
                        RefreshToken = result.RefreshToken,
                    };
                }

                throw new SeeTrueException(HttpStatusCode.BadRequest, "User not found");

            }
        }
    }
}
