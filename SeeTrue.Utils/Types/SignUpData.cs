using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SeeTrue.Utils.Validators;

namespace SeeTrue.Utils.Types
{
    public record SignUpData
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public Dictionary<string, object> UserMetaData { get; set; }
    }
}
