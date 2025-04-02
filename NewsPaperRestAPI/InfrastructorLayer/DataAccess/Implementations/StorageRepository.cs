using InfrastructorLayer.DataAccess.Contracts;
using InfrastructorLayer.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InfrastructorLayer.DataAccess.Implementations
{
    public abstract class StorageRepository<TEntity, TId, TContext> : IStorageRepository<TEntity, TId>
        where TEntity : BaseRecord<TId>
        where TContext : DbContext, new()
    {
        public async Task DeleteAsync(TEntity entity)
        {
            using TContext ctx = new();
            ctx.Set<TEntity>().Remove(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(TId id)
        {
            var entity = await GetByIdAsync(id);
            using TContext ctx = new();
            ctx.Set<TEntity>().Remove(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeList)
        {
            using TContext ctx = new();
            IQueryable<TEntity> query = ctx.Set<TEntity>();
            if (includeList != null && includeList.Length > 0)
            {
                foreach (var include in includeList)
                {
                    query = query.Include(include);
                }
            }
            query = query.Where(predicate);
            return await query.ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(params string[] includeList)
        {
            using TContext ctx = new();
            IQueryable<TEntity> query = ctx.Set<TEntity>();
            if (includeList != null && includeList.Length > 0)
            {
                foreach (var include in includeList)

                    query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeList)
        {
            using TContext ctx = new();
            IQueryable<TEntity> query = ctx.Set<TEntity>();
            if (includeList != null)
            {
                foreach (var item in includeList)
                {
                    query = query.Include(item);
                }
            }
            return await query.SingleOrDefaultAsync(predicate);
        }

        public async Task<TEntity> GetByIdAsync(TId id, params string[] includeList)
        {
            using TContext ctx = new();
            IQueryable<TEntity> query = ctx.Set<TEntity>();
            if (includeList != null)
            {
                foreach (var item in includeList)

                    query = query.Include(item);

            }
            return await query.SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task InsertAsync(TEntity entity)
        {
            using (TContext ctx = new())
            {
                await ctx.Set<TEntity>().AddAsync(entity);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            using TContext ctx = new();
            ctx.Set<TEntity>().Update(entity);
            await ctx.SaveChangesAsync();
        }
    }
}