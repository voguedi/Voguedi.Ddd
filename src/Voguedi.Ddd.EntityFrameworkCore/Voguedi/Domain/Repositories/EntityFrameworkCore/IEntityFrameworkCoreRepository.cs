using Microsoft.EntityFrameworkCore;
using Voguedi.Domain.AggregateRoots;

namespace Voguedi.Domain.Repositories.EntityFrameworkCore
{
    public interface IEntityFrameworkCoreRepository<TAggregateRoot, TIdentity> : IRepository<TAggregateRoot, TIdentity>
        where TAggregateRoot : class, IAggregateRoot<TIdentity>
    {
        #region Properties

        DbContext DbContext { get; }

        DbSet<TAggregateRoot> DbSet { get; }

        #endregion
    }
}
