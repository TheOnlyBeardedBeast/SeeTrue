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
        [Required]
        public NotificationType Type { get; set; }
        [Required]
        public string Language { get; set; }
        public string Template { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Audience { get; set; }
        [Required]
        public string Subject { get; set; }
    }
}
