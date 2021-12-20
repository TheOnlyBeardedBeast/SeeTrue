using System.Linq;
using System.Collections.Generic;
using System;

namespace SeeTrue.Infrastructure.Utils
{
    public static class Env
    {
        public static readonly string SigningKey;
        public static readonly int AccessTokenLifetime;
        public static readonly string Issuer;
        public static readonly List<string> Audiences;
        public static readonly int RefreshTokenLifetime;
        public static readonly string SmtpHost;
        public static readonly int SmtpPort;
        public static readonly string SmtpUser;
        public static readonly string SmtpPass;
        public static readonly int VerificationTokenLifetime;
        public static readonly int RecoveryMaxFrequency;
        public static readonly bool ValidateIssuer;
        public static readonly bool ValidateAudience;

        public static readonly bool SignupDisabled;
        public static readonly Guid InstanceId;
        public static readonly bool AutoConfirm;
        public static readonly string JwtDefaultGroupName;
        static Env()
        {
            SigningKey = Helpers.GetRequiredEnvironmentVariable<string>("SEETRUE_SIGNING_KEY");
            AccessTokenLifetime = Helpers.GetEnvironmentVariable<int>("SEETRUE_TOKEN_LIFETIME", 3600);
            Issuer = Helpers.GetEnvironmentVariable<string>("SEETRUE_ISSUER", "SeeTrue");
            Audiences = Helpers.GetEnvironmentVariable<string>("SEETRUE_AUIDIENCES", null)?.Split(",").ToList() ?? new List<string>();
            RefreshTokenLifetime = Helpers.GetEnvironmentVariable<int>("SEETRUE_REFRESH_TOKEN_LIFETIME", 336);
            SmtpHost = Helpers.GetRequiredEnvironmentVariable<string>("SEETRUE_SMTP_HOST");
            SmtpPort = Helpers.GetRequiredEnvironmentVariable<int>("SEETRUE_SMTP_PORT");
            SmtpUser = Helpers.GetRequiredEnvironmentVariable<string>("SEETRUE_SMTP_USER");
            SmtpPass = Helpers.GetRequiredEnvironmentVariable<string>("SEETRUE_SMTP_PASS");
            VerificationTokenLifetime = Helpers.GetEnvironmentVariable<int>("SEETRUE_VERIFICATION_TOKEN_LIFETIME", 24);
            RecoveryMaxFrequency = Helpers.GetEnvironmentVariable<int>("SEETRUE_RECOVERY_MAX_FREQUENCY", 5);
            ValidateIssuer = Helpers.GetEnvironmentVariable<bool>("SEETRUE_VALIDATE_ISSUER", true);
            ValidateAudience = Helpers.GetEnvironmentVariable<bool>("SEETRUE_VALIDATE_AUDIENCE", true);
            SignupDisabled = Helpers.GetEnvironmentVariable<bool>("SEETRUE_SIGNUP_DISABLED", false);
            InstanceId = Helpers.GetEnvironmentVariable<Guid>("SEETRUE_INSTANCE_ID", Guid.Empty);
            AutoConfirm = Helpers.GetEnvironmentVariable<bool>("SEETRUE_AUTOCONFIRM", false);
            JwtDefaultGroupName = Helpers.GetEnvironmentVariable<string>("SEETRUE_JWT_DEFAULT_GROUP_NAME", "user");

        }
    }
}