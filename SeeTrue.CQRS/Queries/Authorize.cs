using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;

namespace SeeTrue.Infrastructure.Queries
{
    public static class Authorize
    {
        public record Query(string Provider) : IRequest<string>;

        public class Handler : IRequestHandler<Query, string>
        {
            public Task<string> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.Provider != "github")
                {
                    throw new SeeTrueException(HttpStatusCode.BadRequest, "Invalid provider");
                };

                var qb = new QueryBuilder();
                qb.Add("client_id", "github client id");
                qb.Add("scope", "user:email");
                // Cache state with 10 min ttl
                qb.Add("state", Helpers.GenerateUniqueToken());

                return Task.FromResult($"https://github.com/login/oauth/authorize{qb.ToQueryString()}");
            }
        }
    }
}
