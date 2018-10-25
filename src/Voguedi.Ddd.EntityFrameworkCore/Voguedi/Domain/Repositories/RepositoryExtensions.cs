using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Voguedi.Domain.AggregateRoots;
using Voguedi.Domain.Repositories.EntityFrameworkCore;

namespace Voguedi.Domain.Repositories
{
    public static class RepositoryExtensions
    {
        #region Public Methods

        public static IEntityFrameworkCoreRepository<TAggregateRoot, TIdentity> ToEntityFrameworkCoreRepository<TAggregateRoot, TIdentity>(
            this IRepository<TAggregateRoot, TIdentity> repository)
            where TAggregateRoot : class, IAggregateRoot<TIdentity>
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            if (repository is IEntityFrameworkCoreRepository<TAggregateRoot, TIdentity> entityFrameworkCoreRepository)
                return entityFrameworkCoreRepository;

            throw new ArgumentException(nameof(repository));
        }

        public static DbContext GetDbContext<TAggregateRoot, TIdentity>(this IRepository<TAggregateRoot, TIdentity> repository)
            where TAggregateRoot : class, IAggregateRoot<TIdentity>
            => repository.ToEntityFrameworkCoreRepository().DbContext;

        public static DbSet<TAggregateRoot> GetDbSet<TAggregateRoot, TIdentity>(this IRepository<TAggregateRoot, TIdentity> repository)
            where TAggregateRoot : class, IAggregateRoot<TIdentity>
            => repository.ToEntityFrameworkCoreRepository().DbSet;

        public static IQueryable<TAggregateRoot> GetAllIncludeDetails<TAggregateRoot, TIdentity>(
            this IRepository<TAggregateRoot, TIdentity> repository,
            params Expression<Func<TAggregateRoot, object>>[] detailSepcifications)
            where TAggregateRoot : class, IAggregateRoot<TIdentity>
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            var aggregateRoots = repository.GetAll();

            if (detailSepcifications?.Count() > 0)
            {
                foreach (var specification in detailSepcifications)
                    aggregateRoots = aggregateRoots.Include(specification);
            }

            return aggregateRoots;
        }

        #endregion
    }
}
