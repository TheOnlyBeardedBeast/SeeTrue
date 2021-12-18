using System;
using System.ComponentModel.DataAnnotations;

namespace SeeTrue.Models
{
    public class RefreshToken
    {
		public Guid? InstanceID { get; set; } = null;
		[Key]
		public Guid Id { get; set; }
		[Required]
		public string Token { get; set; }
		[Required]
		public Guid UserId { get; set; }
		public User User { get; set; }

		[Required]
		public Guid LoginId { get; set; }
		public Login Login { get; set; }

		public bool Revoked { get; set; } = false;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	}
}
