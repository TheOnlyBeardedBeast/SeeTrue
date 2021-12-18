using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SeeTrue.Models
{
    public class Login
    {
        [Key]
        public Guid Id { get; set; }

        public string UserAgent { get; set; }

        [JsonIgnore]
        public ICollection<RefreshToken> RefreshTokens { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
