using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SeeTrue.API.RequestModels
{
    public record SignUpData
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(8)]
        public string Password { get; set; }

        public Dictionary<string, string> UserMetaData { get; set; }
    }
}
