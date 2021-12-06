using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Models;
using SeeTrue.Utils;
using SeeTrue.Utils.Extensions;
using SeeTrue.Utils.Types;

namespace SeeTrue.CQRS.Commands
{
    public static class SignUp
    {
        public record Command(SignUpData Data, string Aud, string Provider = "email"): IRequest<User>;

        public class Handler : IRequestHandler<Command, User>
        {
            protected readonly SeeTrueDbContext db;
            protected readonly IMediator mediator;

            public Handler(IMediator metiator)
            {
                this.mediator = metiator;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                if (SeeTrueConfig.DisableSignup)
                {
                    throw new Exception("SignUp is disabled");
                }

                if (!request.Data.Validate())
                {
                    throw new Exception("Ivalid data.");
                }
                

                var instanceId = SeeTrueConfig.InstanceId;
                var user = await this.mediator.Send(new CQRS.Queries.FindUser.Query(instanceId, request.Data.Email, request.Aud));

                if(user is not null && user.IsConfirmed())
                {
                    throw new Exception("A user with this email address has already been registered");
                }

                if (SeeTrueConfig.AutoConfirm)
                {
                    
                    // TODO: trigger eventHook
                    user = await this.mediator.Send(new Commands.SignUpNewUser.Command(request.Data.Email, request.Data.Password, request.Aud, request.Provider, request.Data.UserMetaData, true));
                    await this.mediator.Send(new NewAuditLogEntry.Command(instanceId, user, AuditAction.UserSignedUpAction, null));

                } else
                {
                    user = await this.mediator.Send(new Commands.SignUpNewUser.Command(request.Data.Email, request.Data.Password, request.Aud, request.Provider, request.Data.UserMetaData, false));
                    // TODO: send email
                }

                return user;
            }
        }
    }
}
