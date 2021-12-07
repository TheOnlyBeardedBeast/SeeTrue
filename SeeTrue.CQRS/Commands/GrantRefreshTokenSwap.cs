using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Models;
using SeeTrue.Utils;

namespace SeeTrue.CQRS.Commands
{
    public static class GrantRefreshTokenSwap
    {
        public record Command(RefreshToken Token) : IRequest<Token.TokenResponse>;

        public class Handler : IRequestHandler<Command, Token.TokenResponse>
        {
            private readonly SeeTrueDbContext db;
            private readonly IMediator m;

            public Handler(ISeeTrueDbContext db, IMediator m)
            {
                this.db = db as SeeTrueDbContext;
                this.m = m;
            }

            public async Task<Token.TokenResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                request.Token.Revoked = true;
                this.db.RefreshTokens.Update(request.Token);

                var response = await this.m.Send(new Commands.IssueRefreshToken.Command(request.Token.User));
                return response;
            }
        }
    }
}
