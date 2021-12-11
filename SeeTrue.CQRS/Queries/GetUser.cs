using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.CQRS.Services;
using SeeTrue.Models;

namespace SeeTrue.CQRS.Queries
{
    public static class GetUser
    {
        public record Query(Guid UserId) : IRequest<User>;

        public class Handler : IRequestHandler<Query,User>
        {
            private readonly IQueryService query;

            public Handler(IQueryService query)
            {
                this.query = query;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                return await this.query.FindUserById(request.UserId);
            }
        }
    }
}
