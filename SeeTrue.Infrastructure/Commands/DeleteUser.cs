using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;

namespace SeeTrue.Infrastructure.Commands
{
    public static class DeleteUser
    {
        public record Command(Guid UserId) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IQueryService queries;
            private readonly ICommandService commands;

            public Handler(IQueryService queries, ICommandService commands)
            {
                this.queries = queries;
                this.commands = commands;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await queries.FindUserById(request.UserId);

                if (user is null)
                {
                    throw new SeeTrueException(HttpStatusCode.BadRequest, "Invalid data");
                }

                await commands.RemoveUser(user);

                return Unit.Value;
            }
        }
    }
}
