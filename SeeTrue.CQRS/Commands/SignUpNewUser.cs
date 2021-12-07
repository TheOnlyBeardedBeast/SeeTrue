using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Models;
using SeeTrue.Utils;

namespace SeeTrue.CQRS.Commands
{
    public static class SignUpNewUser
    {
        public record Command(string Email, string Password, string Aud, string Provider, Dictionary<string, object> userMetaData, bool Confirmed) : IRequest<User>;

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly SeeTrueDbContext db;

            public Handler(ISeeTrueDbContext db)
            {
                this.db = db as SeeTrueDbContext;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {

                var appMetaData = new Dictionary<string, object>();
                appMetaData["Provider"] = request.Provider;

                var user = new User
                {
                    InstanceID = SeeTrueConfig.InstanceId,
                    Email = request.Email,
                    Aud = request.Aud,
                    UserMetaData = request.userMetaData,
                    EncryptedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    AppMetaData = appMetaData,
                    Role = SeeTrueConfig.JWTDefaultGroupName,
                    ConfirmedAt = DateTime.UtcNow,
                };

                // TODO: Trigger event
                db.Users.Add(user);
                //await db.SaveChangesAsync();

                return user;
            }
        }
    }
}
