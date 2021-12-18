using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;

namespace SeeTrue.Infrastructure.Commands
{
    public static class ProcessMagicLink
    {
        public record Command(string Token) : IRequest<UserTokenResponse>;

        public class Handler : IRequestHandler<Command, UserTokenResponse>
        {
            private readonly IMemoryCache cache;
            private readonly ICommandService commands;
            private readonly IQueryService queries;

            public Handler(IMemoryCache cache,ICommandService commands,IQueryService queries)
            {
                this.cache = cache;
                this.commands = commands;
                this.queries = queries;
            }

            public async Task<UserTokenResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                if(this.cache.TryGetValue(request.Token, out Guid userId))
                {
                    var user = await queries.FindUserById(userId);

                    if(user is null)
                    {
                        goto Error;
                    }

                    this.cache.Remove(request.Token);
                    var result = await this.commands.IssueTokens(user);

                    return new UserTokenResponse {
                        User = user,
                        AccessToken = result.AccessToken,
                        RefreshToken = result.RefreshToken,
                    };
                }

                Error:
                    throw new SeeTrueException(HttpStatusCode.BadRequest, "User not found");

            }
        }
    }
}
