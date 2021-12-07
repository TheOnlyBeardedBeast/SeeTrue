using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Models;
using SeeTrue.Utils;

namespace SeeTrue.CQRS.Commands
{

    public static class NewAuditLogEntry
    {
        public record Command(Guid InstanceId, User Actor, AuditAction Action, object Traits) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            protected readonly SeeTrueDbContext db;

            public Handler(ISeeTrueDbContext db)
            {
                this.db = db as SeeTrueDbContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // Environment.GetEnvironmentVariable("");

                var entry = new AuditLogEntry
                {
                    InstanceId = request.InstanceId,
                    Payload = new Dictionary<string, string>
                    {
                        { "Timestamp", DateTime.UtcNow.ToString() },
                        { "ActorId", request.Actor.Id.ToString() },
                        { "ActorEmail", request.Actor.Email.ToString() },
                        { "Action", request.Action.ToString() },
                        { "LogType", Constants.ActionLogTypeMap[request.Action].ToString() }
                    }
                };


                if (request.Actor.UserMetaData != null && request.Actor.UserMetaData.TryGetValue("FullName", out object fullName))
                {
                    entry.Payload.Add("ActorName", fullName.ToString());
                }

                this.db.AuditLogEntries.Add(entry);
                //await this.db.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}
