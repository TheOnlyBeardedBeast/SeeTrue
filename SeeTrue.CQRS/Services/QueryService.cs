using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SeeTrue.Models;
using SeeTrue.Utils;

namespace SeeTrue.CQRS.Services
{
    public interface IQueryService
    {
        Task<RefreshToken> FindRefreshTokenWithUser(string refreshToken);
        Task<User> FindUserByEmailAndAudience(string email, string audience);
        Task<User> FindUserByConfirmationToken(string confirmationToken);
        Task<User> FindUserById(Guid userId);
        Task<bool> CheckEmailExists(string email, string audience);
        Task<User> FindUserByEmailChangeToken(string token);
    }

    public class QueryService : IQueryService
    {
        private readonly SeeTrueDbContext db;

        public QueryService(ISeeTrueDbContext db)
        {
            this.db = db as SeeTrueDbContext;
        }

        /// <summary>
        /// Returns a User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        public async Task<User> FindUserByEmailAndAudience(string email, string audience)
        {
            var instanceId = SeeTrueConfig.InstanceId;

            var user = await db.Users.Where(e =>
                        e.InstanceID == instanceId &&
                        e.Email == email &&
                        e.Aud == audience)
                    .FirstOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// Gets the user by the ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> FindUserById(Guid userId)
        {
            return await this.db.Users.FirstOrDefaultAsync(e => e.Id == userId);
        }

        /// <summary>
        /// Returns a RefresToken and i'ts User
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<RefreshToken> FindRefreshTokenWithUser(string refreshToken)
        {
            var token = await this.db.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(e => e.Token == refreshToken);
            return token;
        }

        /// <summary>
        /// Returns a user by a confirmation token
        /// </summary>
        /// <param name="confirmationToken"></param>
        /// <returns></returns>
        public async Task<User> FindUserByConfirmationToken(string confirmationToken)
        {
            return await this.db.Users.FirstOrDefaultAsync(e => e.ConfirmationToken.Equals(confirmationToken) && e.InstanceID == SeeTrueConfig.InstanceId);
        }

        /// <summary>
        /// Checks if a user with the same email, audience and instance exists
        /// </summary>
        /// <param name="email"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        public async Task<bool> CheckEmailExists(string email,string audience)
        {
            var instanceId = SeeTrueConfig.InstanceId;

            return await this.db.Users.AnyAsync(e => e.InstanceID == instanceId && e.Email == email && e.Aud == audience);
        }

        public async Task<User> FindUserByEmailChangeToken(string token)
        {
            return await this.db.Users.FirstOrDefaultAsync( e => e.EmailChangeToken == token);
        }

    }
}
