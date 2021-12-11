using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SeeTrue.Utils.Extensions
{
    public static class TypeExtensions
    {
        public static double GetUnixEpoch(this DateTime dateTime)
        {
            var unixTime = dateTime.ToUniversalTime() -
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return unixTime.TotalSeconds;
        }

        public static Guid GetUserId(this HttpContext context)
        {
            var userId = context.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier).Value;

            if(userId is null)
            {
                throw new Exception("Not authenticated");
            }

            if (Guid.TryParse(userId,out var uid))
            {
                return uid;
            }

            throw new Exception("Invalid userId");
        }
    }
}
