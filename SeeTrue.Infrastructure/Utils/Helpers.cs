using System;
using System.Collections.Generic;
using System.Linq;
using Jose;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Utils
{
    public static class Helpers
    {
        public static string GenerateUniqueToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public static string GenerateTimestampedToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();

            return Convert.ToBase64String(time.Concat(key).ToArray());
        }

        public static bool ValidateExpiringToken(string token, int tokenLifetime)
        {
            byte[] data = Convert.FromBase64String(token);
            DateTime timestamp = DateTime.FromBinary(BitConverter.ToInt64(data, 0));

            if (timestamp.AddMinutes(tokenLifetime) < DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }

        public static IEnumerable<string> ParseAudiences(string source)
        {
            return source.Split(",");
        }
    }
}
