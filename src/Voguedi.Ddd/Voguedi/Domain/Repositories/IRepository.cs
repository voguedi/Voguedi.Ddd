using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Voguedi.Domain.AggregateRoots;

namespace Voguedi.Domain.Repositories
{
    public interface IRepository { }

    public interface IRepository<TAggregateRoot, TIdentity> : IRepository
        where TAggregateRoot : class, IAggregateRoot<TIdentity>
    {
        #region Methods

        void Create(TAggregateRoot aggregateRoot);

        Task CreateAsync(TAggregateRoot aggregateRoot);

        void Delete(TAggregateRoot aggregateRoot);

        Task DeleteAsync(TAggregateRoot aggregateRoot);

        void Delete(TIdentity id);

        Task DeleteAsync(TIdentity id);

        void Delete(Expression<Func<TAggregateRoot, bool>> specification);

        Task DeleteAsync(Expression<Func<TAggregateRoot, bool>> specification);

        void Modify(TAggregateRoot aggregateRoot);

        Task ModifyAsync(TAggregateRoot aggregateRoot);

        IQueryable<TAggregateRoot> GetAll();

        IReadOnlyList<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<IReadOnlyList<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        TAggregateRoot FindFirst(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<TAggregateRoot> FindFirstAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        TAggregateRoot FindSingle(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<TAggregateRoot> FindSingleAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        TAggregateRoot Find(TIdentity id);

        Task<TAggregateRoot> FindAsync(TIdentity id);

        int CountAll(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<int> CountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        long LongCountAll(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<long> LongCountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        bool Exists(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        #endregion
    }
}
