﻿using System;
using System.Collections.Generic;

namespace SeeTrue.Infrastructure.Types
{
    public record SignUpRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public Dictionary<string, object> UserMetaData { get; set; }
    }
}