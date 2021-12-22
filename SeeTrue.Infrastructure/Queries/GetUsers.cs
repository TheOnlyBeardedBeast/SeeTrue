using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Queries
{
    public static class GetUsers
    {
        public record Query(int Page = 1, int PerPage = 20) : IRequest<List<User>>;

        public class Handler : IRequestHandler<Query, List<User>>
        {
            private readonly IQueryService queries;

            public Handler(IQueryService queries)
            {
                this.queries = queries;
            }

            public async Task<List<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await queries.PaginateUsers(request.Page, request.PerPage);
            }
        }
    }
}
