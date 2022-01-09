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

        public static T GetEnvironmentVariable<T>(string key, T defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            try
            {
                return (T)Convert.ChangeType(Environment.GetEnvironmentVariable(key), typeof(T));
            }
            catch (System.Exception)
            {
                throw new Exception($"Invalid Environment variable {key}");
            }
        }

        public static T GetRequiredEnvironmentVariable<T>(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception($"Missing required Environment variable {key}");
            }

            try
            {
                return (T)Convert.ChangeType(Environment.GetEnvironmentVariable(key), typeof(T));
            }
            catch (System.Exception)
            {
                throw new Exception($"Invalid required Environment variable {key}");
            }
        }

        public static IDictionary<string, int> ConvertToDictionary<T>() where T : struct
        {
            var dictionary = new Dictionary<string, int>();

            var values = Enum.GetValues(typeof(T));

            foreach (var value in values)
            {
                int key = (int)value;

                dictionary.Add(value.ToString(), key);
            }

            return dictionary;
        }
    }
}
