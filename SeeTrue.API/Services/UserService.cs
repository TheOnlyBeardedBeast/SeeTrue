using System;
using System.Collections.Generic;
using System.Text.Json;
using SeeTrue.Models;

namespace SeeTrue.API.Services
{
    public static class UserService
    {
        public static User NewUser(Guid instanceId, string email, string password, string aud, Dictionary<string, object> userMetaData)
        {
            var id = Guid.NewGuid();
            var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                InstanceID = instanceId,
                Id = id,
                Aud = aud,
                Email = email,
                UserMetaData = userMetaData,
                EncryptedPassword = encryptedPassword,
            };

            return user;
        }

        public static User NewSystemUser(Guid instanceId, string aud)
        {
            return new User
            {
                InstanceID = instanceId,
                Id = Guid.Empty,
                Aud = aud,
                IsSuperAdmin = true,
            };
        }
    }

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

        public static void UpdateUserMetaData(this User user, Dictionary<string,object> userMetaData)
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
    }
}

//missing
// setrole, UpdateUserMetaData, UpdateAppMetaData