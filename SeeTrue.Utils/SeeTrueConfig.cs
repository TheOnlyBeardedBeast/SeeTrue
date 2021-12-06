using System;
namespace SeeTrue.Utils
{
    public static class SeeTrueConfig
    {
        public static bool DisableSignup = false;
        public static Guid InstanceId = Guid.Empty;
        public static bool AutoConfirm = true;
        public static string JWTDefaultGroupName = "user";
        public static string WebhookURL = null;
    }
}
