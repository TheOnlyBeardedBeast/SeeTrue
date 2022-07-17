using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Models;
using UserMetaData = System.Collections.Generic.Dictionary<string, string>;

namespace SeeTrue.Infrastructure.Commands
{
    public static class UserUpdate
    {
        public record Command(Guid UserId, string Password, string Email, UserMetaData UserMetaData) : IRequest<User>;

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly IQueryService queries;
            private readonly ICommandService commands;

            public Handler(IQueryService queries, ICommandService commands)
            {
                this.queries = queries;
                this.commands = commands;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await queries.FindUserById(request.UserId);

                if (user is null)
                {
                    throw new SeeTrueException(HttpStatusCode.BadRequest, "User not found");
                }

                if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    await commands.UpdateUserPassword(user, request.Password);
                }

                if (request.UserMetaData is not null)
                {
                    await commands.UpdateUserMetaData(user, request.UserMetaData);
                }

                if (!string.IsNullOrWhiteSpace(request.Email) && user.Email != request.Email)
                {
                    var emailExists = await queries.CheckEmailExists(request.Email, user.Aud);

                    if (emailExists)
                    {
                        throw new SeeTrueException(HttpStatusCode.BadRequest, "Email already in use");
                    }

                    await commands.SendEmailChange(user, request.Email);
                }

                return user;
            }
        }
    }
}
