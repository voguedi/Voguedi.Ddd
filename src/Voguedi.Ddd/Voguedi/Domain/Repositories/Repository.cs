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

        public virtual int CountAll(Expression<Func<TAggregateRoot, bool>> specification = null) => specification != null ? GetAll().Where(specification).Count() : GetAll().Count();

        public virtual Task<int> CountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null) => Task.FromResult(CountAll(specification));

        public abstract void Create(TAggregateRoot aggregateRoot);

        public virtual Task CreateAsync(TAggregateRoot aggregateRoot)
        {
            Create(aggregateRoot);
            return Task.CompletedTask;
        }

        public abstract void Delete(TAggregateRoot aggregateRoot);

        public virtual Task DeleteAsync(TAggregateRoot aggregateRoot)
        {
            Delete(aggregateRoot);
            return Task.CompletedTask;
        }

        public virtual void Delete(TIdentity id)
        {
            var aggregateRoot = GetAll().FirstOrDefault(BuildIdEqualsSepcification(id));

            if (aggregateRoot != null)
                Delete(aggregateRoot);
        }

        public virtual Task DeleteAsync(TIdentity id)
        {
            Delete(id);
            return Task.CompletedTask;
        }

        public virtual void Delete(Expression<Func<TAggregateRoot, bool>> specification)
        {
            foreach (var aggregateRoot in GetAll().Where(specification))
                Delete(aggregateRoot);
        }

        public virtual Task DeleteAsync(Expression<Func<TAggregateRoot, bool>> specification)
        {
            Delete(specification);
            return Task.CompletedTask;
        }

        public virtual bool Exists(Expression<Func<TAggregateRoot, bool>> specification = null) => specification != null ? GetAll().Any(specification) : GetAll().Any();

        public virtual Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => Task.FromResult(Exists(specification));

        public virtual TAggregateRoot Find(TIdentity id) => GetAll().FirstOrDefault(BuildIdEqualsSepcification(id));

        public virtual IReadOnlyList<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? GetAll().Where(specification).ToList() : GetAll().ToList();

        public virtual Task<IReadOnlyList<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null) => Task.FromResult(FindAll(specification));

        public virtual Task<TAggregateRoot> FindAsync(TIdentity id) => Task.FromResult(Find(id));

        public virtual TAggregateRoot FindFirst(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? GetAll().FirstOrDefault(specification) : GetAll().FirstOrDefault();

        public virtual Task<TAggregateRoot> FindFirstAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => Task.FromResult(FindFirst(specification));

        public virtual TAggregateRoot FindSingle(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? GetAll().SingleOrDefault(specification) : GetAll().SingleOrDefault();

        public virtual Task<TAggregateRoot> FindSingleAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => Task.FromResult(FindSingle(specification));

        public abstract IQueryable<TAggregateRoot> GetAll();

        public virtual long LongCountAll(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? GetAll().Where(specification).LongCount() : GetAll().LongCount();

        public virtual Task<long> LongCountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null) => Task.FromResult(LongCountAll(specification));

        public abstract void Modify(TAggregateRoot aggregateRoot);

        public virtual Task ModifyAsync(TAggregateRoot aggregateRoot)
        {
            Modify(aggregateRoot);
            return Task.CompletedTask;
        }
        
        #endregion
    }
}
