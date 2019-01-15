using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Voguedi.AspectCore;
using Voguedi.Domain.AggregateRoots;

namespace Voguedi.Domain.Repositories
{
    public interface IRepository<TAggregateRoot, TIdentity>
        where TAggregateRoot : class, IAggregateRoot<TIdentity>
    {
        #region Methods

        void Create([NotNull] TAggregateRoot aggregateRoot);

        Task CreateAsync([NotNull] TAggregateRoot aggregateRoot);

        void Delete([NotNull] TAggregateRoot aggregateRoot);

        Task DeleteAsync([NotNull] TAggregateRoot aggregateRoot);

        void Modify([NotNull] TAggregateRoot aggregateRoot);

        Task ModifyAsync([NotNull] TAggregateRoot aggregateRoot);

        IQueryable<TAggregateRoot> GetAll();

        IReadOnlyList<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<IReadOnlyList<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        TAggregateRoot FindFirst(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<TAggregateRoot> FindFirstAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        TAggregateRoot FindSingle(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<TAggregateRoot> FindSingleAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        TAggregateRoot Find([NotNull] TIdentity id);

        Task<TAggregateRoot> FindAsync([NotNull] TIdentity id);

        int CountAll(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<int> CountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        long LongCountAll(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<long> LongCountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        bool Exists(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        #endregion
    }
}
