using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SeeTrue.Models;

namespace SeeTrue.CQRS.Queries
{
    public static class FindUser
    {
        public record Query(Guid InstanceId, string Email, string Aud) : IRequest<User>;

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly SeeTrueDbContext db;

            public Handler(ISeeTrueDbContext db)
            {
                this.db = db as SeeTrueDbContext;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await db.Users.Where(e =>
                        e.InstanceID == request.InstanceId &&
                        e.Email == request.Email &&
                        e.Aud == request.Aud)
                    .FirstOrDefaultAsync();

                return user;
            }
        }
    }
}
