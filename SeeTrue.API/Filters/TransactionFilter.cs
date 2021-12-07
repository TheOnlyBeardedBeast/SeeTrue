using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using SeeTrue.API.Db;

namespace SeeTrue.API.Filters
{
    public class TransactionFilter : IAsyncActionFilter
    {
        private readonly AppDbContext _dbContext;

        public TransactionFilter(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                _dbContext.BeginTransaction();

                await next();

                await _dbContext.CommitTransactionAsync();
            }
            catch (Exception)
            {
                _dbContext.RollbackTransaction();
                throw;
            }
        }
    }
}
