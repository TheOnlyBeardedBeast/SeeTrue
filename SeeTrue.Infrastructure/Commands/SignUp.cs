using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Extensions;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Commands
{
    public static class SignUp
    {
        public record Command(SignUpData Data, string Aud, string Provider = "email") : IRequest<User>;

        public class Handler : IRequestHandler<Command, User>
        {
            protected readonly SeeTrueDbContext db;
            private readonly IQueryService query;
            private readonly ICommandService command;

            public Handler(IQueryService query, ICommandService command)
            {
                this.query = query;
                this.command = command;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                var instanceId = Env.InstanceId;
                var user = await query.FindUserByEmailAndAudience(request.Data.Email, request.Aud);

                if (user is not null && user.IsConfirmed())
                {
                    throw new SeeTrueException(HttpStatusCode.BadRequest, "A user with this email address has already been registered");
                }

                if (Env.AutoConfirm)
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
