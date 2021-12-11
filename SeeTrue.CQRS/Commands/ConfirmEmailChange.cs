using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.CQRS.Services;

namespace SeeTrue.CQRS.Commands
{
    public static class ConfirmEmailChange
    {
        public record Command(string Token): IRequest;

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
                var user = await queries.FindUserByEmailChangeToken(request.Token);

                if (user is null)
                {
                    throw new Exception("invalid token");
                }

                await commands.ConfirmEmailChange(user);

                return Unit.Value;
            }
        }
    }
}
