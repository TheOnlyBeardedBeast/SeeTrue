using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Utils;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Commands
{
    public static class AdminAddEmail
    {
        public record Command(string Audience, NotificationType Type, string Subject, string Language, string Content, string Template) : IRequest<Mail>;

        public class Handler : IRequestHandler<Command, Mail>
        {
            private readonly ICommandService commands;

            public Handler(ICommandService commands)
            {
                this.commands = commands;
            }

            public async Task<Mail> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await commands.CreateMail(new Mail
                {
                    InstanceId = Env.InstanceId,
                    Audience = request.Audience,
                    Type = request.Type,
                    Language = request.Language,
                    Template = request.Template,
                    Content = request.Content,
                    Subject = request.Subject
                });

                return result;
            }
        }
    }
}
