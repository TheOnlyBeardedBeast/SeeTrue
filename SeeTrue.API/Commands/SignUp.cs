using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SeeTrue.API.Services;
using SeeTrue.API.Utils;
using SeeTrue.Models;

namespace SeeTrue.API.Commands
{
    public static class SignUp
    {
        public record Command(string Email, string Password, Dictionary<string, string> Data, string Aud): IRequest<User>;

        public class Handler : IRequestHandler<Command, User>
        {
            protected readonly SeeTrueDbContext db;
            protected readonly IMediator mediator;

            public Handler(ISeeTrueDbContext db, IMediator metiator)
            {
                this.db = db as SeeTrueDbContext;
                this.mediator = metiator;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                if (SeeTrueConfig.DisableSignup)
                {
                    throw new Exception("Signups not allowed for this instance");
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    throw new Exception("Signup requires a valid password");
                }

                if(string.IsNullOrWhiteSpace(request.Email))
                {
                    throw new Exception("Signup requires a valid email");
                }

                var instanceId = SeeTrueConfig.InstanceId;

                var user = await db.Users.Where(e => e.InstanceID == instanceId && e.Email == request.Email && e.Aud == request.Aud).FirstOrDefaultAsync();


                if(user is not null)
                {
                    if(user.IsConfirmed())
                    {
                        throw new Exception("A user with this email address has already been registered");
                    }

                    if (user.UserMetaData is null)
                    {
                        user.UserMetaData = request.Data;

                        db.Users.Update(user);
                    } else if(request.Data is not null)
                    {
                        foreach (KeyValuePair<string, string> entry in request.Data)
                        {
                            if(entry.Value!=null)
                            {
                                user.UserMetaData[entry.Key] = entry.Value;
                            } else
                            {
                                user.UserMetaData.Remove(entry.Key);
                            }
                        }

                        db.Users.Update(user);
                    }


                } else
                {
                    user = await this.mediator.Send(new Commands.SignUpNewUser.Command(request.Email, request.Password, request.Aud, "email", request.Data));
                }

                if (SeeTrueConfig.AutoConfirm)
                {
                    await this.mediator.Send(new NewAuditLogEntry.Command(instanceId, user, AuditAction.UserSignedUpAction, null));
                    // TODO: trigger eventHook
                    user.ConfirmedAt = DateTime.UtcNow;
                    this.db.Users.Update(user);

                } else
                {
                    // TODO: send email
                }

                await db.SaveChangesAsync();
                return user;
            }
        }
    }
}
