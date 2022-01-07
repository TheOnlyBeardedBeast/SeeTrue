using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Queries
{
    public static class GetMails
    {
        public record Query(int Page = 1, int PerPage = 20) : IRequest<Pagination<Mail>>;

        public class Handler : IRequestHandler<Query, Pagination<Mail>>
        {
            private readonly IQueryService queries;

            public Handler(IQueryService queries)
            {
                this.queries = queries;
            }

            public async Task<Pagination<Mail>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await queries.PaginateMails(request.Page, request.PerPage);
            }
        }
    }
}
