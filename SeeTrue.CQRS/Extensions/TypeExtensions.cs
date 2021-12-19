using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Jose;
using Microsoft.AspNetCore.Http;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsConfirmed(this User user)
        {
            return user.ConfirmedAt.HasValue;
        }

        public static bool HasRole(this User user, string role)
        {
            return user.Role == role;
        }

        public static void UpdateUserMetaData(this User user, Dictionary<string, object> userMetaData)
        {
            if (user.UserMetaData is null)
            {
                user.UserMetaData = userMetaData;

                //db.Users.Update(user);
            }
            else if (userMetaData is not null)
            {
                foreach (KeyValuePair<string, object> entry in userMetaData)
                {
                    if (entry.Value != null)
                    {
                        user.UserMetaData[entry.Key] = entry.Value;
                    }
                    else
                    {
                        user.UserMetaData.Remove(entry.Key);
                    }
                }

                //db.Users.Update(user);
            }
        }

        public static string GenerateAccessToken(this User user)
        {
            // TODO: Get lifetime from env
            var payload = new Dictionary<string, object> {
                { "sub", user.Id.ToString() },
                { "aud", user.Aud },
                { "iss", "http://localhost:5000/" },
                { "exp", (int)DateTime.UtcNow.AddSeconds(3600).GetUnixEpoch() },
                { "lid", Guid.NewGuid().ToString() }
            };

            var headers = new Dictionary<string, object>{
                { "email", user.Email },
                { "app_metadata", user.AppMetaData},
                { "user_metadata", user.UserMetaData }
            };
            // TODO: dynamic secret key
            var secretKey = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };

            return Jose.JWT.Encode(payload, secretKey, JwsAlgorithm.HS256, extraHeaders: headers);
        }

        public static double GetUnixEpoch(this DateTime dateTime)
        {
            var unixTime = dateTime.ToUniversalTime() -
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return unixTime.TotalSeconds;
        }

        public static Guid GetUserId(this HttpContext context)
        {
            var userId = context.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier).Value;

            if (userId is null)
            {
                throw new SeeTrueException(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            if (Guid.TryParse(userId, out var uid))
            {
                return uid;
            }

            throw new SeeTrueException(HttpStatusCode.BadRequest, "Invalid userId");
        }

        public static Guid GetLoginId(this HttpContext context)
        {
            var loginId = context.User.Claims.FirstOrDefault(e => e.Type == "lid").Value;

            if (loginId is null)
            {
                throw new SeeTrueException(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            if (Guid.TryParse(loginId, out var uid))
            {
                return uid;
            }

            throw new SeeTrueException(HttpStatusCode.BadRequest, "Invalid loginId");
        }
    }
}
