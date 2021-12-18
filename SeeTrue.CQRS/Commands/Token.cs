using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Extensions;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;

namespace SeeTrue.Infrastructure.Commands
{
    public static class Token
    {
        public record Command(TokenData data, string Aud) : IRequest<UserTokenResponse>;

        public class Handler : IRequestHandler<Command, UserTokenResponse>
        {
            private readonly IMediator mediator;
            private readonly IQueryService query;
            private readonly ICommandService command;

            public Handler(IMediator mediator, IQueryService query, ICommandService command)
            {
                this.mediator = mediator;
                this.query = query;
                this.command = command;
            }

            public async Task<UserTokenResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var instanceId = SeeTrueConfig.InstanceId;

                UserTokenResponse response = null;

                switch (request.data.GrantType)
                {
                    case "password":
                        {
                            response = await this.HandlePassword(request, instanceId);
                            break;
                        }
                    case "refresh_token":
                        {
                            response = await this.HandleRefresh(request, instanceId);
                            break;
                        }
                    default:
                        {
                            throw new Exception("Invalid data");
                        }
                }

                // TODO: handle cookie
                return response;
            }

            public async Task<UserTokenResponse> HandlePassword(Command request, Guid instanceId)
            {
                var user = await query.FindUserByEmailAndAudience(request.data.Email, request.Aud);

                if (user is null)
                {
                    throw new Exception("Invalid user or password");
                }

                if (!user.IsConfirmed())
                {
                    throw new Exception("Invalid user or password");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.data.Password, user.EncryptedPassword))
                {
                    throw new Exception("Invalid user or password");
                }

                await command.NewAuditLogEntry(user, AuditAction.LoginAction, null);
                var token = await command.IssueTokens(user);

                return new UserTokenResponse
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    User = user
                };
            }

            public async Task<UserTokenResponse> HandleRefresh(Command request, Guid instanceId)
            {
                var token = await query.FindRefreshTokenWithUser(request.data.RefreshToken);

                if (token is null || token.User is null)
                {
                    throw new Exception("Invalid data");
                }

                if (token.Revoked)
                {
                    // TODO: Clear cookie token
                    throw new Exception("Invalid data");
                }

                var validationDate = token.CreatedAt;

                // TODO: Get refresh token lifetime from config
                if (validationDate.AddDays(14) < DateTime.UtcNow)
                {
                    //alternative validation (date is embedded into the token)
                    //!Helpers.ValidateExpiringToken(request.data.RefreshToken, 7 * 24 * 60
                    throw new Exception("Invalid data");
                }

                await command.NewAuditLogEntry(token.User, AuditAction.TokenRefreshedAction, null);
                var response = await command.GrantTokenSwap(token);

                return new UserTokenResponse
                {
                    AccessToken = response.AccessToken,
                    RefreshToken = response.RefreshToken,
                    User = token.User
                };
            }
        }
    }
}
