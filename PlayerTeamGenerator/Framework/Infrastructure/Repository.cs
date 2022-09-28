using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using PlayerTeamGenerator.Helpers;
using static PlayerTeamGenerator.Framework.Application.IRepository;

namespace PlayerTeamGenerator.Framework.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> dbSet;
        public Repository(DataContext context)
        {
            dbSet = context.Set<T>();
        }

        public virtual T Add(T entity)
        {
            dbSet.Add(entity);
            return entity;
        }

        public virtual IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
            return entities;
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = dbSet;

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (orderBy is not null)
            {
                query = orderBy(dbSet);
            }

            return query;
        }

        public virtual IQueryable<T> Get<TProperty>(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, TProperty>>[] includedProperties)
        {
            IQueryable<T> query = dbSet;


            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includedProperty in includedProperties)
            {
                query = query.Include(includedProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public virtual async Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = dbSet;

            if (orderBy is not null)
            {
                query = orderBy(dbSet);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T> GetFirstAsync<TProperty>(Expression<Func<T, bool>> predicate = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           params Expression<Func<T, TProperty>>[] includedProperties)
        {
            IQueryable<T> query = dbSet;

            foreach (var includedProperty in includedProperties)
            {
                query = query.Include(includedProperty);
            }

            if (orderBy is not null)
            {
                query = orderBy(dbSet);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public virtual void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public virtual T Update(T entity)
        {
            dbSet.Update(entity);
            return entity;
        }

        public virtual IEnumerable<T> UpdateRange(IEnumerable<T> entities)
        {
            dbSet.UpdateRange(entities);
            return entities;
        }
    }
}
