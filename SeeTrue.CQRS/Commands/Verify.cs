using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Commands
{
    public static class Verify
    {
        public record Command(string type,string token, string password) : IRequest<UserTokenResponse>;

        public class Handler : IRequestHandler<Command, UserTokenResponse>
        {
            private readonly IQueryService query;
            private readonly ICommandService command;

            public Handler(IQueryService query, ICommandService command)
            {
                this.query = query;
                this.command = command;
            }

            public async Task<UserTokenResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                User user;
                switch (request.type) {
                    case "signup":
                        {
                            user = await this.SignupVerify(request);
                            break;
                        }
                    case "recovery":
                        {
                            user = await this.RecoveryVerify(request);
                            break;
                        }
                    default:
                        {
                            user = null;
                            break;
                        }
                }

                if(user is null)
                {
                    throw new Exception("Invalid input");
                }

                var token = await command.IssueTokens(user);

                return new UserTokenResponse
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    ExpiresIn = token.ExpiresIn,
                    TokenType = token.TokenType,
                    User = user
                };
            }

            public async Task<User> SignupVerify(Command request)
            {
                var user = await query.FindUserByConfirmationToken(request.token);

                if (user is null || user.ConfirmedAt != null)
                {
                    throw new Exception("Invalid data");
                }

                
                if(string.IsNullOrWhiteSpace(user.EncryptedPassword))
                {
                    if(user.InvitedAt is not null)
                    {
                        if (string.IsNullOrWhiteSpace(request.password))
                        {
                            throw new Exception("Invalid data");
                        }

                        await command.UpdateUserPassword(user, request.password);
                    }
                }

                // TODO: trigger event
                await command.NewAuditLogEntry(user, AuditAction.UserSignedUpAction, null);
                await command.ConfirmUser(user);

                return user;
            }

            public async Task<User> RecoveryVerify(Command request)
            {
                var user = await query.FindUserByRecoveryToken(request.token);
                
                if (user is null)
                {
                    throw new Exception("Invalid data");
                }

                await this.command.Recover(user);

                return user;
            }
        }
    }
}
