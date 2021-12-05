using System;
using System.Collections.Generic;
using System.Text.Json;
using SeeTrue.Models;

namespace SeeTrue.API.Services
{
    public static class UserService
    {
        public static User NewUser(Guid instanceId, string email, string password, string aud, Dictionary<string, string> userMetaData)
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
    }
}

//missing
// setrole, UpdateUserMetaData, UpdateAppMetaData