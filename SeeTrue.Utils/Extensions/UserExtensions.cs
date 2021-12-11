using System;
using System.Collections.Generic;
using Jose;
using SeeTrue.Models;

namespace SeeTrue.Utils.Extensions
{
    public static class UserExtensions
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
                { "exp", DateTime.UtcNow.AddSeconds(3600).GetUnixEpoch() }
                //{ "exp", ((int)DateTime.Now.AddSeconds(3600).ToUniversalTime().Subtract(new DateTime (1970, 01, 01)).TotalSeconds) }
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

    }
}
