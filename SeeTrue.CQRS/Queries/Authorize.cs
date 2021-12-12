using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using SeeTrue.Utils;

namespace SeeTrue.CQRS.Queries
{
    public static class Authorize
    {
        public record Query(string Provider) : IRequest<string>;

        public class Handler : IRequestHandler<Query,string>
        {
            public async Task<string> Handle(Query request, CancellationToken cancellationToken)
            {
                if(request.Provider != "github")
                {
                    throw new Exception("Invalid provider");
                };

                var qb = new QueryBuilder();
                qb.Add("client_id", "github client id");
                qb.Add("scope", "user:email");
                // Cache state with 10 min ttl
                qb.Add("state", Helpers.GenerateUniqueToken());

                return $"https://github.com/login/oauth/authorize{qb.ToQueryString()}";
            }
        }
    }
}
