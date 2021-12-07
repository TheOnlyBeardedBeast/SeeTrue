using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SeeTrue.Models;

namespace SeeTrue.CQRS.Queries
{
    public static class FindUserByRefreshToken
    {
        public record Query(string Token) : IRequest<RefreshToken>;

        public class Handler : IRequestHandler<Query, RefreshToken>
        {
            private readonly SeeTrueDbContext db;

            public Handler(ISeeTrueDbContext db)
            {
                this.db = db as SeeTrueDbContext;
            }

            public async Task<RefreshToken> Handle(Query request, CancellationToken cancellationToken)
            {
                var token = await this.db.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(e => e.Token == request.Token);
                return token;
            }
        }
    }
}
