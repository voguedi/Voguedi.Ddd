using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Voguedi.Domain.AggregateRoots;

namespace Voguedi.Domain.Repositories
{
    public interface IRepository<TAggregateRoot, TIdentity>
        where TAggregateRoot : class, IAggregateRoot<TIdentity>
    {
        #region Methods

        void Create(TAggregateRoot aggregateRoot);

        void Delete(TAggregateRoot aggregateRoot);

        void Modify(TAggregateRoot aggregateRoot);

        IQueryable<TAggregateRoot> GetAll();

        Task<List<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<TAggregateRoot> FindFirstAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<TAggregateRoot> FindSingleAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<TAggregateRoot> FindAsync(TIdentity id);

        Task<int> CountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<long> LongCountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        Task<bool> ExistsAsyncAsync(Expression<Func<TAggregateRoot, bool>> specification = null);

        #endregion
    }
}
