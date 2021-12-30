using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeeTrue.Infrastructure.Extensions;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;
using SeeTrue.Models;
using SeeTrue.Utils.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace SeeTrue.Infrastructure.Services
{
    public interface ICommandService
    {
        /// <summary>
        /// Creates a new User in the database and returns its result
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="audience"></param>
        /// <param name="provider"></param>
        /// <param name="userMetaData"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        Task<User> SignUpNewUser(string email, string password, string audience, string provider, Dictionary<string, object> userMetaData, bool confirmed);

        /// <summary>
        /// Revokes the current refresh token and generates new tokens
        /// </summary>
        /// <param name="token">Current refresh token</param>
        /// <returns></returns>
        Task<TokenResponse> GrantTokenSwap(RefreshToken token);

        /// <summary>
        /// Generates new Acces and Refresh tokens
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<TokenResponse> IssueTokens(User user, Guid loginId);

        /// <summary>
        /// Creates a new audit log entry in the db
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="action"></param>
        /// <param name="traits"></param>
        /// <returns></returns>
        Task NewAuditLogEntry(User actor, AuditAction action, object traits);


        /// <summary>
        /// Sends a confirmation email to the user and updates related data in the db
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task SendConfirmation(User user);


        /// <summary>
        /// Updates the users encrypted password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task UpdateUserPassword(User user, string password);

        /// <summary>
        /// Confirms the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task ConfirmUser(User user);

        /// <summary>
        /// Updateds the user metada, adds new fields, or modifies fields, if the new field has a value off null removes fields
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userMetaData"></param>
        /// <returns></returns>
        Task UpdateUserMetaData(User user, Dictionary<string, object> userMetaData);

        /// <summary>
        /// Sends an email change token email to the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task SendEmailChange(User user, string email);

        /// <summary>
        /// Confrims the email change
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task ConfirmEmailChange(User user);

        /// <summary>
        /// Send a password recovery token email to the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task SendPasswordRecovery(User user);

        /// <summary>
        /// Updates the u
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task Recover(User user);

        /// <summary>
        /// Stores a login session in the db
        /// </summary>
        /// <param name="userAgent"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Login> StoreLogin(string userAgent, Guid userId);

        /// <summary>
        /// Invalidates refresh token by a loginId
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        Task InvalidateRefreshTokenByLoginId(Guid loginId);

        /// <summary>
        /// Restricts the usage of active access tokens for example after logout
        /// </summary>
        /// <param name="loginId"></param>
        void RestrictAccesTokenUsage(Guid loginId);

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key"></param>
        void RemoveFromCache(string key);

        /// <summary>
        /// Adds item to the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        TItem SetCache<TItem>(object key, TItem value);

        /// <summary>
        /// Adds item to the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl">Relative time to live</param>
        TItem SetCache<TItem>(object key, TItem value, TimeSpan ttl);

        /// <summary>
        /// Removes a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task RemoveUser(User user);

        /// <summary>
        /// Updates a users email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task UpdateUserEmail(User user, string email);

        /// <summary>
        /// Updates the AppMetaData for a given user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="appMetaData"></param>
        /// <returns></returns>
        Task UpdateAppMetaData(User user, Dictionary<string, object> appMetaData);

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        Task<User> CreateUser(AdminUpdateUserRequest userData);

        Task<User> InviteUser(string email, string Aud, string provider);
    }

    public class CommandService : ICommandService
    {
        private readonly SeeTrueDbContext db;
        private readonly IMailService mailer;
        private readonly IMemoryCache cache;

        public CommandService(ISeeTrueDbContext db, IMailService mailer, IMemoryCache cache)
        {
            this.db = db as SeeTrueDbContext;
            this.mailer = mailer;
            this.cache = cache;
        }

        public async Task<User> SignUpNewUser(string email, string password, string audience, string provider, Dictionary<string, object> userMetaData, bool confirmed)
        {
            var appMetaData = new Dictionary<string, object>();
            appMetaData["provider"] = provider;

            var user = new User
            {
                InstanceID = Env.InstanceId,
                Email = email,
                Aud = audience,
                UserMetaData = userMetaData,
                EncryptedPassword = BCrypt.Net.BCrypt.HashPassword(password),
                AppMetaData = appMetaData,
                Role = Env.JwtDefaultGroupName,
                ConfirmedAt = confirmed ? DateTime.UtcNow : null,
            };

            // TODO: Trigger event
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return user;
        }

        public async Task<TokenResponse> GrantTokenSwap(RefreshToken token)
        {
            token.Revoked = true;
            this.db.RefreshTokens.Update(token);
            await db.SaveChangesAsync();

            var response = await this.IssueTokens(token.User, token.LoginId);
            return response;
        }

        public async Task<TokenResponse> IssueTokens(User user, Guid loginId)
        {
            user.LastSignInAt = DateTime.UtcNow;

            var refresToken = Helpers.GenerateTimestampedToken();
            var accessToken = user.GenerateAccessToken(loginId);

            db.RefreshTokens.Add(new RefreshToken
            {
                Token = refresToken,
                InstanceID = user.InstanceID,
                UserId = user.Id,
                LoginId = loginId
            });

            await db.SaveChangesAsync();

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refresToken,
                ExpiresIn = 3600,
            };
        }

        public async Task NewAuditLogEntry(User actor, AuditAction action, object traits)
        {
            var entry = new AuditLogEntry
            {
                InstanceId = Env.InstanceId,
                Payload = new Dictionary<string, string>
                    {
                        { "Timestamp", DateTime.UtcNow.ToString() },
                        { "ActorId", actor?.Id.ToString() ?? Guid.Empty.ToString() },
                        { "ActorEmail", actor?.Email.ToString() ?? "" },
                        { "Action", action.ToString() },
                        { "LogType", Constants.ActionLogTypeMap[action].ToString() }
                    }
            };


            if (actor?.UserMetaData != null && actor.UserMetaData.TryGetValue("full_name", out object fullName))
            {
                entry.Payload.Add("actore_name", fullName.ToString());
            }

            this.db.AuditLogEntries.Add(entry);
            await this.db.SaveChangesAsync();
        }

        public async Task SendConfirmation(User user)
        {
            if (user.ConfirmationSentAt != null && user.ConfirmationSentAt > DateTime.UtcNow.AddHours(0 - Env.VerificationTokenLifetime))
            {
                return;
            }

            user.ConfirmationToken = Helpers.GenerateUniqueToken();
            user.ConfirmationSentAt = DateTime.UtcNow;

            await mailer.Send(user.Email, user.ConfirmationToken);

            db.Update(user);
            await db.SaveChangesAsync();
        }

        public async Task UpdateUserPassword(User user, string password)
        {
            user.EncryptedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            db.Update(user);

            await db.SaveChangesAsync();
        }

        public async Task ConfirmUser(User user)
        {
            user.ConfirmationToken = null;
            user.ConfirmedAt = DateTime.UtcNow;

            this.db.Update(user);
            await db.SaveChangesAsync();
        }


        public async Task UpdateUserMetaData(User user, Dictionary<string, object> userMetaData)
        {
            user.UpdateUserMetaData(userMetaData);
            this.db.Update(user);

            await this.db.SaveChangesAsync();
        }

        public async Task UpdateAppMetaData(User user, Dictionary<string, object> appMetaData)
        {
            user.UpdateAppMetaData(appMetaData);
            this.db.Update(user);

            await this.db.SaveChangesAsync();
        }

        public async Task SendEmailChange(User user, string email)
        {
            user.EmailChangeToken = Helpers.GenerateUniqueToken();
            user.EmailChange = email;

            await this.mailer.NotifyEmailChange(user, email);

            user.EmailChangeSentAt = DateTime.UtcNow;

            this.db.Update(user);
            await this.db.SaveChangesAsync();
        }

        public async Task ConfirmEmailChange(User user)
        {
            user.Email = user.EmailChange;
            user.EmailChangeToken = null;
            user.EmailChange = null;

            this.db.Update(user);
            await this.db.SaveChangesAsync();
        }

        public async Task SendPasswordRecovery(User user)
        {
            if (user.RecoverySentAt < DateTime.UtcNow.AddMinutes(Env.RecoveryMaxFrequency))
            {
                throw new SeeTrueException(HttpStatusCode.BadRequest, "Try again later");
            }

            user.RecoveryToken = Helpers.GenerateUniqueToken();
            await mailer.NotifyRecovery(user);

            user.RecoverySentAt = DateTime.UtcNow;

            this.db.Update(user);
            await this.db.SaveChangesAsync();
        }

        public async Task Recover(User user)
        {
            user.RecoveryToken = null;
            this.db.Update(user);

            await this.db.SaveChangesAsync();
        }

        public async Task<Login> StoreLogin(string userAgent, Guid userId)
        {
            var login = new Login
            {
                UserAgent = userAgent,
                UserId = userId
            };

            this.db.Logins.Add(login);
            await this.db.SaveChangesAsync();

            return login;
        }

        public async Task InvalidateRefreshTokenByLoginId(Guid loginId)
        {
            var refreshTokensToUpdate = await this.db.RefreshTokens.Where(e => e.LoginId == loginId && e.Revoked != false).ToListAsync();

            refreshTokensToUpdate.ForEach(e =>
            {
                e.Revoked = true;
                e.UpdatedAt = DateTime.UtcNow;
            });

            await this.db.SaveChangesAsync();
        }

        public void RestrictAccesTokenUsage(Guid loginId)
        {
            this.cache.Set(loginId.ToString(), loginId, TimeSpan.FromSeconds(Env.AccessTokenLifetime));
        }

        public void RemoveFromCache(string key)
        {
            this.cache.Remove(key);
        }

        public TItem SetCache<TItem>(object key, TItem value)
        {
            return this.cache.Set(key, value);
        }

        public TItem SetCache<TItem>(object key, TItem value, TimeSpan ttl)
        {
            return this.cache.Set(key, value, ttl);
        }

        public async Task RemoveUser(User user)
        {
            this.db.Users.Remove(user);

            await this.db.SaveChangesAsync();
        }

        public async Task UpdateUserEmail(User user, string email)
        {
            user.Email = email;
            this.db.Users.Update(user);

            await this.db.SaveChangesAsync();
        }

        public async Task<User> CreateUser(AdminUpdateUserRequest userData)
        {
            var user = new User
            {
                Email = userData.Email,
                EncryptedPassword = BCrypt.Net.BCrypt.HashPassword(userData.Password),
                Aud = Env.Audiences[0],
                Role = userData.Role ?? Env.JwtDefaultGroupName,
                UserMetaData = userData.UserMetaData,
                AppMetaData = userData.AppMetaData,
                ConfirmedAt = (userData.Confirm.HasValue && userData.Confirm.Value) ? DateTime.UtcNow : null,
            };

            this.db.Add(user);
            await this.db.SaveChangesAsync();

            return user;
        }

        public async Task<User> InviteUser(string email, string Aud, string provider)
        {
            var appMetaData = new Dictionary<string, object>();
            appMetaData["provider"] = provider;

            var user = new User
            {
                InstanceID = Env.InstanceId,
                Aud = Aud,
                Email = email,
                AppMetaData = appMetaData,
                Role = Env.JwtDefaultGroupName,
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return user;
        }
    }
}
