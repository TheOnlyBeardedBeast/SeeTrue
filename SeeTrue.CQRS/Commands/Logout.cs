using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;

namespace SeeTrue.Infrastructure.Commands
{
    public static class Logout
    {
        public record Command(Guid LoginId): IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IQueryService queries;
            private readonly ICommandService commands;

            public Handler(IQueryService queries, ICommandService commands)
            {
                this.queries = queries;
                this.commands = commands;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await commands.InvalidateRefreshTokenByLoginId(request.LoginId);
                commands.RestrictAccesTokenUsage(request.LoginId);

                return Unit.Value;
            }
        }
    }
}
