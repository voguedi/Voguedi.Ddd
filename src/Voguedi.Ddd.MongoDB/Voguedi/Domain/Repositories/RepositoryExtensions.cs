using System;
using MongoDB.Driver;
using Voguedi.Domain.AggregateRoots;
using Voguedi.Domain.Repositories.MongoDB;

namespace Voguedi.Domain.Repositories
{
    public static class RepositoryExtensions
    {
        #region Public Methods

        public static IMongoDBRepository<TAggregateRoot, TIdentity> ToMongoDBRepository<TAggregateRoot, TIdentity>(this IRepository<TAggregateRoot, TIdentity> repository)
            where TAggregateRoot : class, IAggregateRoot<TIdentity>
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            if (repository is MongoDBRepository<TAggregateRoot, TIdentity> mongoDBRepository)
                return mongoDBRepository;

            throw new ArgumentException(nameof(repository));
        }

        public static IMongoDatabase GetDatabase<TAggregateRoot, TIdentity>(this IRepository<TAggregateRoot, TIdentity> repository)
            where TAggregateRoot : class, IAggregateRoot<TIdentity>
            => repository.ToMongoDBRepository().Database;

        public static IMongoCollection<TAggregateRoot> GetCollection<TAggregateRoot, TIdentity>(this IRepository<TAggregateRoot, TIdentity> repository)
            where TAggregateRoot : class, IAggregateRoot<TIdentity>
            => repository.ToMongoDBRepository().Collection;

        public static IClientSessionHandle GetSession<TAggregateRoot, TIdentity>(this IRepository<TAggregateRoot, TIdentity> repository)
            where TAggregateRoot : class, IAggregateRoot<TIdentity>
            => repository.ToMongoDBRepository().Session;

        #endregion
    }
}
