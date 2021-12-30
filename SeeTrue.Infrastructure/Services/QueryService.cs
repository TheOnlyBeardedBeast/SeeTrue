using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Services
{
    public interface IQueryService
    {
        Task<RefreshToken> FindRefreshTokenWithUser(string refreshToken);
        Task<User> FindUserByEmailAndAudience(string email, string audience);
        Task<User> FindUserByConfirmationToken(string confirmationToken);
        Task<User> FindUserById(Guid userId);
        Task<bool> CheckEmailExists(string email, string audience);
        Task<User> FindUserByEmailChangeToken(string token);
        Task<User> FindUserByRecoveryToken(string token);
        bool TryGetValueFromCache<T>(object key, out T value);
        Task<Pagination<User>> PaginateUsers(int page = 1, int perPage = 20);
    }

    public class QueryService : IQueryService
    {
        private readonly SeeTrueDbContext db;
        private readonly IMemoryCache cache;

        public QueryService(ISeeTrueDbContext db, IMemoryCache cache)
        {
            this.db = db as SeeTrueDbContext;
            this.cache = cache;
        }

        /// <summary>
        /// Returns a User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        public async Task<User> FindUserByEmailAndAudience(string email, string audience)
        {
            var instanceId = Env.InstanceId;

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
            return await this.db.Users.FirstOrDefaultAsync(e => e.ConfirmationToken.Equals(confirmationToken) && e.InstanceID == Env.InstanceId);
        }

        /// <summary>
        /// Checks if a user with the same email, audience and instance exists
        /// </summary>
        /// <param name="email"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        public async Task<bool> CheckEmailExists(string email, string audience)
        {
            var instanceId = Env.InstanceId;

            return await this.db.Users.AnyAsync(e => e.InstanceID == instanceId && e.Email == email && e.Aud == audience);
        }

        public async Task<User> FindUserByEmailChangeToken(string token)
        {
            return await this.db.Users.FirstOrDefaultAsync(e => e.EmailChangeToken == token);
        }

        public async Task<User> FindUserByRecoveryToken(string token)
        {
            return await this.db.Users.FirstOrDefaultAsync(e => e.RecoveryToken == token);
        }

        public bool TryGetValueFromCache<T>(object key, out T value)
        {
            return this.cache.TryGetValue<T>(key, out value);
        }

        public async Task<Pagination<User>> PaginateUsers(int page = 1, int perPage = 20)
        {
            var items = await this.db.Users.OrderBy(x => x.Id).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            var count = await this.db.Users.CountAsync();

            return new Pagination<User>(page, perPage, count, items);
        }

    }
}
