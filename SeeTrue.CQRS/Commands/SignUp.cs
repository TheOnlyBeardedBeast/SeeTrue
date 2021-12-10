using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.CQRS.Services;
using SeeTrue.Models;
using SeeTrue.Utils;
using SeeTrue.Utils.Extensions;
using SeeTrue.Utils.Types;

namespace SeeTrue.CQRS.Commands
{
    public static class SignUp
    {
        public record Command(SignUpData Data, string Aud, string Provider = "email") : IRequest<User>;

        public class Handler : IRequestHandler<Command, User>
        {
            protected readonly SeeTrueDbContext db;
            protected readonly IMediator mediator;
            private readonly IQueryService query;
            private readonly ICommandService command;

            public Handler(IMediator metiator, IQueryService query, ICommandService command)
            {
                this.mediator = metiator;
                this.query = query;
                this.command = command;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                var instanceId = SeeTrueConfig.InstanceId;
                var user = await query.FindUserByEmailAndAudience(request.Data.Email, request.Aud);

                if (user is not null && user.IsConfirmed())
                {
                    throw new Exception("A user with this email address has already been registered");
                }

                if (SeeTrueConfig.AutoConfirm)
                {
                    // TODO: trigger eventHook
                    user = await command.SignUpNewUser(request.Data.Email, request.Data.Password, request.Aud, request.Provider, request.Data.UserMetaData, true);
                    await command.NewAuditLogEntry(user, AuditAction.UserSignedUpAction, null);

                }
                else
                {
                    user = await command.SignUpNewUser(request.Data.Email, request.Data.Password, request.Aud, request.Provider, request.Data.UserMetaData, false);
                    await command.SendConfirmation(user);
                }

                return user;
            }
        }
    }
}
