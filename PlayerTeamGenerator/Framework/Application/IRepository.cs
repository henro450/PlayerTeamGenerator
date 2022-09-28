using System.Linq.Expressions;

namespace PlayerTeamGenerator.Framework.Application
{
    public interface IRepository
    {
        public interface IRepository<T> where T : class
        {
            T Add(T entity);

            IEnumerable<T> AddRange(IEnumerable<T> entities);

            IQueryable<T> Get(
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
                );

            IQueryable<T> Get<TProperty>(
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                params Expression<Func<T, TProperty>>[] includedProperties
                );

            Task<T> GetFirstAsync(
                Expression<Func<T, bool>> predicate = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
                );

            Task<T> GetFirstAsync<TProperty>(
                Expression<Func<T, bool>> predicate = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                params Expression<Func<T, TProperty>>[] includedProperties
                );

            void Remove(T entity);

            void RemoveRange(IEnumerable<T> entities);

            T Update(T entity);

            IEnumerable<T> UpdateRange(IEnumerable<T> entities);
        }
    }
}
