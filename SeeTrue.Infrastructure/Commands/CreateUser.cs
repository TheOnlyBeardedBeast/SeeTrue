using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Commands
{
    public static class CreateUser
    {
        public record Command : AdminUpdateUserRequest, IRequest<User>;

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
                var user = await queries.FindUserByEmailAndAudience(request.Email, Env.Audiences[0]);

                if (user is not null)
                {
                    throw new SeeTrueException(HttpStatusCode.BadRequest, "User already exists");
                }

                user = await this.commands.CreateUser(request);

                return user;
            }
        }
    }
}
