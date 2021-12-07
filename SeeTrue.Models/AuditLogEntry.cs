using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeeTrue.Models
{
    public class AuditLogEntry
    {
        [Key]
        public Guid Id { get; set; }

        public Guid InstanceId { get; set; }

        [Column(TypeName = "jsonb")]
        public Dictionary<string, string> Payload { get; set; }

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}
