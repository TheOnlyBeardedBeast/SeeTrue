using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;

namespace SeeTrue.Infrastructure.Commands
{
    public static class AdminDeleteMail
    {
        public record Command(Guid Id) : IRequest;

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
                var mail = await queries.GetMail(request.Id);

                if(mail is null)
                {
                    throw new SeeTrueException(System.Net.HttpStatusCode.BadRequest, "Invalid data");
                }

                await commands.RemoveEmail(mail);

                return Unit.Value;
            }
        }
    }
}
