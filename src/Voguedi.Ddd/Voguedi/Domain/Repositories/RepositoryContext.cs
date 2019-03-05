using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Voguedi.DisposableObjects;
using Voguedi.Domain.AggregateRoots;
using Voguedi.Utils;

namespace Voguedi.Domain.Repositories
{
    public abstract class RepositoryContext<TDbContext> : DisposableObject, IRepositoryContext<TDbContext>
        where TDbContext : class
    {
        #region Private Fields

        readonly ConcurrentDictionary<Type, IRepository> repositoryMapping;

        #endregion

        #region Ctors

        protected RepositoryContext(TDbContext dbContext)
        {
            DbContext = dbContext;
            Id = ObjectId.NewObjectId().ToString();
            repositoryMapping = new ConcurrentDictionary<Type, IRepository>();
        }

        #endregion

        #region Protected Methods

        protected abstract IRepository<TAggregateRoot, TIdentity> CreateRepository<TAggregateRoot, TIdentity>() where TAggregateRoot : class, IAggregateRoot<TIdentity>;

        #endregion

        #region DisposableObject

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                repositoryMapping.Clear();
        }

        #endregion

        #region IRepositoryContext<TDbContext>

        public TDbContext DbContext { get; }

        public string Id { get; }

        object IRepositoryContext.DbContext => DbContext;

        public virtual void Commit() { }

        public virtual Task CommitAsync()
        {
            Commit();
            return Task.CompletedTask;
        }

        IRepository<TAggregateRoot, TIdentity> IRepositoryContext.GetRepository<TAggregateRoot, TIdentity>()
            => (IRepository<TAggregateRoot, TIdentity>)repositoryMapping.GetOrAddIfNotNull(typeof(TAggregateRoot), CreateRepository<TAggregateRoot, TIdentity>());

        #endregion
    }
}
