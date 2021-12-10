using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeeTrue.CQRS.Types;
using SeeTrue.Models;
using SeeTrue.Utils;
using SeeTrue.Utils.Extensions;
using SeeTrue.Utils.Services;

namespace SeeTrue.CQRS.Services
{
    public interface ICommandService
    {
        Task<User> SignUpNewUser(string email, string password, string audience, string provider, Dictionary<string, object> userMetaData, bool confirmed);
        Task<TokenResponse> GrantTokenSwap(RefreshToken token);
        Task<TokenResponse> IssueTokens(User user);
        Task NewAuditLogEntry(User actor, AuditAction action, object traits);
        Task SendConfirmation(User user);
        Task UpdateUserPassword(User user, string password);
        Task ConfirmUser(User user);
    }

    public class CommandService : ICommandService
    {
        private readonly SeeTrueDbContext db;
        private readonly IMailService mailer;

        public CommandService(ISeeTrueDbContext db, IMailService mailer)
        {
            this.db = db as SeeTrueDbContext;
            this.mailer = mailer;
        }

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
        public async Task<User> SignUpNewUser(string email, string password, string audience, string provider, Dictionary<string, object> userMetaData, bool confirmed)
        {
            var appMetaData = new Dictionary<string, object>();
            appMetaData["provider"] = provider;

            var user = new User
            {
                InstanceID = SeeTrueConfig.InstanceId,
                Email = email,
                Aud = audience,
                UserMetaData = userMetaData,
                EncryptedPassword = BCrypt.Net.BCrypt.HashPassword(password),
                AppMetaData = appMetaData,
                Role = SeeTrueConfig.JWTDefaultGroupName,
                ConfirmedAt = confirmed ? DateTime.UtcNow : null,
            };

            // TODO: Trigger event
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Revokes the current refresh token and generates new tokens
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TokenResponse> GrantTokenSwap(RefreshToken token)
        {
            token.Revoked = true;
            this.db.RefreshTokens.Update(token);
            await db.SaveChangesAsync();

            var response = await this.IssueTokens(token.User);
            return response;
        }

        /// <summary>
        /// Generates new Acces and Refresh tokens
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<TokenResponse> IssueTokens(User user)
        {
            user.LastSignInAt = DateTime.UtcNow;

            var refresToken = Helpers.GenerateTimestampedToken();
            var accessToken = user.GenerateAccessToken();

            db.RefreshTokens.Add(new RefreshToken
            {
                Token = refresToken,
                InstanceID = user.InstanceID,
                UserId = user.Id
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
            // Environment.GetEnvironmentVariable("");

            var entry = new AuditLogEntry
            {
                InstanceId = SeeTrueConfig.InstanceId,
                Payload = new Dictionary<string, string>
                    {
                        { "Timestamp", DateTime.UtcNow.ToString() },
                        { "ActorId", actor.Id.ToString() },
                        { "ActorEmail", actor.Email.ToString() },
                        { "Action", action.ToString() },
                        { "LogType", Constants.ActionLogTypeMap[action].ToString() }
                    }
            };


            if (actor.UserMetaData != null && actor.UserMetaData.TryGetValue("full_name", out object fullName))
            {
                entry.Payload.Add("actore_name", fullName.ToString());
            }

            this.db.AuditLogEntries.Add(entry);
            await this.db.SaveChangesAsync();
        }

        public async Task SendConfirmation(User user)
        {
            Console.WriteLine("Email sent");
            // TODO: lifetime from configuration
            if (user.ConfirmationSentAt != null && user.ConfirmationSentAt > DateTime.UtcNow.AddDays(-1))
            {
                return;
            }

            // TODO: get referer from config
            // string referer = "http://localhost/confirm/"; // for link generation
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
    }
}
