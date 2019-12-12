using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Voguedi.Domain.AggregateRoots;

namespace Voguedi.Domain.Repositories
{
    public abstract class RepositoryContext<TDbContext> : DisposableObject, IRepositoryContext<TDbContext>
        where TDbContext : class
    {
        #region Private Fields

        readonly ConcurrentDictionary<Type, object> repositoryMapping;

        #endregion

        #region Ctors

        protected RepositoryContext() => repositoryMapping = new ConcurrentDictionary<Type, object>();

        #endregion

        #region Protected Methods

        protected abstract IRepository<TAggregateRoot, TIdentity> CreateRepository<TAggregateRoot, TIdentity>() where TAggregateRoot : class, IAggregateRoot<TIdentity>;

        #endregion

        #region IRepositoryContext<TDbContext>

        public TDbContext DbContext { get; }

        object IRepositoryContext.DbContext => DbContext;

        public virtual Task CommitAsync() => Task.CompletedTask;

        IRepository<TAggregateRoot, TIdentity> IRepositoryProvider.GetRepository<TAggregateRoot, TIdentity>()
            => (IRepository<TAggregateRoot, TIdentity>)repositoryMapping.GetOrAdd(typeof(TAggregateRoot), CreateRepository<TAggregateRoot, TIdentity>());

        #endregion
    }
}
