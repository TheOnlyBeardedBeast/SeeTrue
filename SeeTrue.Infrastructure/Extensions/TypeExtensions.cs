using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Jose;
using Microsoft.AspNetCore.Http;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;
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

        public static void UpdateUserMetaData(this User user, Dictionary<string, string> userMetaData)
        {
            if (user.UserMetaData is null)
            {
                user.UserMetaData = userMetaData;
            }
            else if (userMetaData is not null)
            {
                foreach (KeyValuePair<string, string> entry in userMetaData)
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
            }
        }

        public static void UpdateAppMetaData(this User user, Dictionary<string, string> appMetaData)
        {
            if (user.AppMetaData is null)
            {
                user.AppMetaData = appMetaData;
            }
            else if (appMetaData is not null)
            {
                foreach (KeyValuePair<string, string> entry in appMetaData)
                {
                    if (entry.Value != null)
                    {
                        user.AppMetaData[entry.Key] = entry.Value;
                    }
                    else
                    {
                        user.AppMetaData.Remove(entry.Key);
                    }
                }
            }
        }

        public static string GenerateAccessToken(this User user, Guid loginId)
        {
            var payload = new Dictionary<string, object> {
                { "sub", user.Id.ToString() },
                { "aud", user.Aud },
                { "iss",  Env.Issuer },
                { "exp", (int)DateTime.UtcNow.AddSeconds(Env.AccessTokenLifetime).GetUnixEpoch() },
                { "lid", loginId.ToString() },
                { "role", user.Role },
                { "gid", Guid.NewGuid() }
            };

            var headers = new Dictionary<string, object>{
                { "email", user.Email },
                { "app_metadata", user.AppMetaData},
                { "user_metadata", user.UserMetaData }
            };

            return Jose.JWT.Encode(payload, Env.SigningKey.ToByteArray(), JwsAlgorithm.HS256, extraHeaders: headers);
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

        public static string GetUserAgent(this HttpContext context)
        {
            return context.Request.Headers["User-Agent"];
        }

        public static string GetAudience(this HttpContext context)
        {
            return context.Request.Headers["X-JWT-AUD"];
        }

        public static byte[] ToByteArray(this string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }
    }
}
