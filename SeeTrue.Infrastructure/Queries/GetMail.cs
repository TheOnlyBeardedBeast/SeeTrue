using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Queries
{
    public static class GetMail
    {
        public record Query(Guid Id) : IRequest<Mail>;

        public class Handler : IRequestHandler<Query, Mail>
        {
            private readonly IQueryService queries;

            public Handler(IQueryService queries)
            {
                this.queries = queries;
            }

            public async Task<Mail> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await queries.GetMail(request.Id);

                if (result is null)
                {
                    throw new SeeTrueException(System.Net.HttpStatusCode.BadRequest, "Invalid input");
                }

                return result;
            }
        }
    }
}
