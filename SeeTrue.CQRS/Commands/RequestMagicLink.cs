using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;
using SeeTrue.Utils.Services;

namespace SeeTrue.Infrastructure.Commands
{
    public static class RequestMagicLink
    {
        public record Command(string Email) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IQueryService queries;
            private readonly IMailService mailer;
            private readonly ICommandService commands;

            public Handler(IQueryService queries, ICommandService commands, IMailService mailer)
            {
                this.queries = queries;
                this.mailer = mailer;
                this.commands = commands;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await this.queries.FindUserByEmailAndAudience(request.Email, null);

                if (user is null)
                {
                    throw new SeeTrueException(HttpStatusCode.BadRequest, "User not found");
                }

                var token = Helpers.GenerateUniqueToken();

                this.commands.SetCache(token, user.Id, TimeSpan.FromMinutes(2));

                await mailer.NotifyMagicLink(user, token);

                return Unit.Value;
            }
        }
    }
}
