using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.API.Services;
using SeeTrue.Models;

namespace SeeTrue.API.Commands
{
    public static class SignUpNewUser
    {
        public record Command(string Email, string Password, string Aud, string Provider, Dictionary<string,string> userMetaData) : IRequest<User>;

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly SeeTrueDbContext db;

            public Handler(ISeeTrueDbContext db)
            {
                this.db = db as SeeTrueDbContext;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {

                var appMetaData = new Dictionary<string, string>();
                appMetaData["Provider"] = request.Provider;

                var user = new User
                {
                    InstanceID = SeeTrueConfig.InstanceId,
                    Email = request.Email,
                    Aud = request.Aud,
                    UserMetaData = request.userMetaData,
                    EncryptedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    AppMetaData = appMetaData,
                    Role = SeeTrueConfig.JWTDefaultGroupName
                };

                // TODO: Trigger event
                db.Users.Add(user);
                await db.SaveChangesAsync();

                return user;
            }
        }
    }
}
