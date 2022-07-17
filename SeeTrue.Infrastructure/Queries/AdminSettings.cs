using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Response = SeeTrue.Infrastructure.Types.AdminSettings;

namespace SeeTrue.Infrastructure.Queries
{
    public static class AdminSettings
    {
        public record Query(): IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Response());
            }
        }
    }
}
