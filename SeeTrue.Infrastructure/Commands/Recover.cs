using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;

namespace SeeTrue.Infrastructure.Commands
{
    public static class Recover
    {
        public record Command(string Email, string Audience) : IRequest;

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
                var user = await this.queires.FindUserByEmailAndAudience(request.Email, request.Audience);

                if (user is null)
                {
                    throw new SeeTrueException(HttpStatusCode.BadRequest, "Invalid data");
                }

                await commands.NewAuditLogEntry(user, AuditAction.UserRecoveryRequestedAction, null);

                await this.commands.SendPasswordRecovery(user);

                return Unit.Value;
            }
        }
    }
}
