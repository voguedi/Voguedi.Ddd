using MongoDB.Driver;
using Voguedi.Domain.AggregateRoots;

namespace Voguedi.Domain.Repositories.MongoDB
{
    public interface IMongoDBRepository<TAggregateRoot, TIdentity> : IRepository<TAggregateRoot, TIdentity>
        where TAggregateRoot : class, IAggregateRoot<TIdentity>
    {
        #region Properties

        IMongoDatabase Database { get; }

        IMongoCollection<TAggregateRoot> Collection { get; }

        IClientSessionHandle Session { get; }

        #endregion
    }
}
