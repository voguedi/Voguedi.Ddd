using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Voguedi.Domain.AggregateRoots;

namespace Voguedi.Domain.Repositories
{
    public abstract class Repository<TAggregateRoot, TIdentity> : IRepository<TAggregateRoot, TIdentity>
        where TAggregateRoot : class, IAggregateRoot<TIdentity>
    {
        #region Protected Methods

        protected static Expression<Func<TAggregateRoot, bool>> BuildIdEqualsSepcification(TIdentity id)
        {
            var parameter = Expression.Parameter(typeof(TAggregateRoot));
            var body = Expression.Equal(Expression.PropertyOrField(parameter, "Id"), Expression.Constant(id, typeof(TIdentity)));
            return Expression.Lambda<Func<TAggregateRoot, bool>>(body, parameter);
        }

        #endregion

        #region IRepository<TAggregateRoot, TIdentity>

        public virtual Task<int> CountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
        {
            var result = specification != null ? GetAll().Where(specification).Count() : GetAll().Count();
            return Task.FromResult(result);
        }

        public abstract void Create(TAggregateRoot aggregateRoot);

        public abstract void Delete(TAggregateRoot aggregateRoot);

        public virtual Task<bool> ExistsAsyncAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
        {
            var result = specification != null ? GetAll().Any(specification) : GetAll().Any();
            return Task.FromResult(result);
        }

        public virtual Task<List<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
        {
            var result = specification != null ? GetAll().Where(specification).ToList() : GetAll().ToList();
            return Task.FromResult(result);
        }

        public virtual Task<TAggregateRoot> FindAsync(TIdentity id)
        {
            var result = GetAll().FirstOrDefault(BuildIdEqualsSepcification(id));
            return Task.FromResult(result);
        }

        public virtual Task<TAggregateRoot> FindFirstAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
        {
            var result = specification != null ? GetAll().FirstOrDefault(specification) : GetAll().FirstOrDefault();
            return Task.FromResult(result);
        }

        public virtual Task<TAggregateRoot> FindSingleAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
        {
            var result = specification != null ? GetAll().SingleOrDefault(specification) : GetAll().SingleOrDefault();
            return Task.FromResult(result);
        }

        public abstract IQueryable<TAggregateRoot> GetAll();

        public virtual Task<long> LongCountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
        {
            var result = specification != null ? GetAll().Where(specification).LongCount() : GetAll().LongCount();
            return Task.FromResult(result);
        }

        public abstract void Modify(TAggregateRoot aggregateRoot);

        #endregion
    }
}
