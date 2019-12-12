using Voguedi.Domain.AggregateRoots;

namespace Voguedi.Domain.Repositories
{
    public interface IRepositoryProvider
    {
        #region Methods

        IRepository<TAggregateRoot, TIdentity> GetRepository<TAggregateRoot, TIdentity>() where TAggregateRoot : class, IAggregateRoot<TIdentity>;

        #endregion
    }
}
