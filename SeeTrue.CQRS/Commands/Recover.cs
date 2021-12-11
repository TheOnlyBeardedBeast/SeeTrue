using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.CQRS.Services;
using SeeTrue.Utils;

namespace SeeTrue.CQRS.Commands
{
    public static class Recover
    {
        public record Command(string Email): IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IQueryService queires;
            private readonly ICommandService commands;

            public Handler(IQueryService queires, ICommandService commands)
            {
                this.queires = queires;
                this.commands = commands;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await this.queires.FindUserByEmailAndAudience(request.Email, null);

                if(user is null)
                {
                    throw new Exception("Invalid token");
                }

                await commands.NewAuditLogEntry(user, AuditAction.UserRecoveryRequestedAction, null);

                await this.commands.SendPasswordRecovery(user);

                return Unit.Value;
            }
        }
    }
}
