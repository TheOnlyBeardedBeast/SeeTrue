using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Utils;

namespace SeeTrue.Infrastructure.Queries
{
    public static class GetSettings
    {
        public record Query : IRequest<Response> { };

        public class Handler : IRequestHandler<Query, Response>
        {
            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Response { SignupDisabled = Env.SignupDisabled, Autoconfirm = Env.AutoConfirm });
            }
        }

        public record Response
        {
            [JsonPropertyName("signup_disabled")]
            public bool SignupDisabled { get; init; }
            [JsonPropertyName("autoconfirm")]
            public bool Autoconfirm { get; init; }
        }
    }
}