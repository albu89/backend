using CE_API_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace CE_API_V2.Data.Extensions
{
    public static class ContextExtensions
    {
        public static bool IsOrderAttached<TContext, TEntity>(this TContext context, TEntity entity)
    where TContext : DbContext
    where TEntity : BiomarkerOrderModel
        {
            return context.Set<TEntity>().Local.Any(e => e.BiomarkerId == entity.BiomarkerId && e.UserId == entity.UserId);
        }
    }
}