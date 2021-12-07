using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Models;
using SeeTrue.Utils;
using SeeTrue.Utils.Extensions;

namespace SeeTrue.CQRS.Commands
{
    public static class IssueRefreshToken
    {
        public record Command(User user) : IRequest<Token.TokenResponse>;

        public class Handler : IRequestHandler<Command, Token.TokenResponse>
        {
            private readonly SeeTrueDbContext db;

            public Handler(ISeeTrueDbContext db)
            {
                this.db = db as SeeTrueDbContext;
            }

            public async Task<Token.TokenResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                request.user.LastSignInAt = DateTime.UtcNow;

                var refresToken = Helpers.GenerateTimestampedToken();
                var accessToken = request.user.GenerateAccessToken();

                db.RefreshTokens.Add(new RefreshToken
                {
                    Token = refresToken,
                    InstanceID = request.user.InstanceID,
                    UserId = request.user.Id
                });

                return new Token.TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refresToken,
                    ExpiresIn = 3600,
                };
            }
        }
    }
}
