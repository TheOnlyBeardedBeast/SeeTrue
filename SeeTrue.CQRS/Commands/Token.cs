using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Models;
using SeeTrue.Utils;
using SeeTrue.Utils.Extensions;
using SeeTrue.Utils.Types;

namespace SeeTrue.CQRS.Commands
{
    public static class Token
    {
        public record Command(TokenData data, string Aud) : IRequest<Response>;

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IMediator mediator;

            public Handler(IMediator mediator)
            {
                this.mediator = mediator;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var instanceId = SeeTrueConfig.InstanceId;

                Response response = null;

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
                Console.WriteLine("Succesful login");

                return response;
            }

            public async Task<Response> HandlePassword(Command request, Guid instanceId)
            {
                var user = await mediator.Send(new Queries.FindUser.Query(instanceId, request.data.Email, request.Aud));

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

                await this.mediator.Send(new Commands.NewAuditLogEntry.Command(instanceId, user, AuditAction.LoginAction, null));
                var token = await this.mediator.Send(new Commands.IssueRefreshToken.Command(user));

                return new Response
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    User = user
                };
            }

            public async Task<Response> HandleRefresh(Command request, Guid instanceId)
            {
                var token = await this.mediator.Send(new Queries.FindUserByRefreshToken.Query(request.data.RefreshToken));

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

                await this.mediator.Send(new Commands.NewAuditLogEntry.Command(instanceId, token.User, AuditAction.TokenRefreshedAction, null));
                var response = await this.mediator.Send(new GrantRefreshTokenSwap.Command(token));

                return new Response
                {
                    AccessToken = response.AccessToken,
                    RefreshToken = response.RefreshToken,
                    User = token.User
                };
            }
        }

        public record Response : TokenResponse
        {
            public User User { get; init; }
        }

        public record TokenResponse
        {
            public string AccessToken { get; init; }

            public string TokenType { get; init; } = "Bearer";

            public int ExpiresIn { get; init; } = 3600;

            public string RefreshToken { get; init; }
        }
    }
}
