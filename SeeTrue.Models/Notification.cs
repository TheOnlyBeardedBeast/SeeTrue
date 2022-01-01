using System;
using System.ComponentModel.DataAnnotations;

namespace SeeTrue.Models
{
    public enum NotificationType
    {
        Confirmation,
        EmailChange,
        InviteUser,
        MagicLink,
        Recovery
    }

    public class Notification
    {
        [Key]
        public Guid Id { get; set; }
        public NotificationType Type { get; set; }
        public string Language { get; set; }
        public string Template { get; set; }
        public string Content { get; set; }
        public string Audience { get; set; }
    }
}
