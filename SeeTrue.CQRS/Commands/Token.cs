using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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
                if (!request.data.Validate())
                {
                    throw new Exception("Invalid data");
                }

                var instanceId = SeeTrueConfig.InstanceId;

                switch (request.data.GrantType)
                {
                    case "password":
                        {
                            var user = await mediator.Send(new Queries.FindUser.Query(instanceId, request.data.Email, request.Aud));

                            if(user is null)
                            {
                                throw new Exception("Invalid user or password");
                            }

                            if (!user.IsConfirmed())
                            {
                                throw new Exception("Invalid user or password");
                            }

                            if(!BCrypt.Net.BCrypt.Verify(request.data.Password, user.EncryptedPassword))
                            {
                                throw new Exception("Invalid user or password");
                            }



                            break;
                        }
                    case "refresh_token":
                        {
                            break;
                        }
                }

                return null;
            }
        }

        public record Response
        {
            public string AccessToken { get; init; }

            public string TokenType { get; init; }

            public string ExpiresIn { get; init; }

            public string RefreshToken { get; init; }
        }
    }
}
