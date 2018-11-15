using System;
using System.Threading.Tasks;
using Voguedi.Domain.AggregateRoots;

namespace Voguedi.Domain.Repositories
{
    public interface IRepositoryContext : IDisposable
    {
        #region Properties

        string Id { get; }

        object DbContext { get; }

        #endregion

        #region Methods

        IRepository<TAggregateRoot, TIdentity> GetRepository<TAggregateRoot, TIdentity>() where TAggregateRoot : class, IAggregateRoot<TIdentity>;

        void Commit();

        Task CommitAsync();

        #endregion
    }

    public interface IRepositoryContext<TDbContext> : IRepositoryContext
        where TDbContext : class
    {
        #region Properties

        new TDbContext DbContext { get; }

        #endregion
    }
}
