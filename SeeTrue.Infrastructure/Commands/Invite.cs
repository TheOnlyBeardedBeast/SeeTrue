using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;

namespace SeeTrue.Infrastructure.Commands
{
    public static class Invite
    {
        public record Command(string Email, string Aud) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly ICommandService commands;
            private readonly IQueryService queries;

            public Handler(ICommandService commands, IQueryService queries)
            {
                this.commands = commands;
                this.queries = queries;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var exists = await queries.CheckEmailExists(request.Email, request.Aud);

                if (exists)
                {
                    throw new SeeTrueException(HttpStatusCode.BadRequest, "User already exists");
                }

                await commands.InviteUser(request.Email, request.Aud, "email");

                return Unit.Value;
            }
        }
    }
}
