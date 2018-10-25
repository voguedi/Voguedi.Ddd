using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voguedi.Domain.AggregateRoots;

namespace Voguedi.Domain.Repositories.EntityFrameworkCore
{
    class EntityFrameworkCoreRepository<TAggregateRoot, TIdentity> : Repository<TAggregateRoot, TIdentity>, IEntityFrameworkCoreRepository<TAggregateRoot, TIdentity>
        where TAggregateRoot : class, IAggregateRoot<TIdentity>
    {
        #region Ctors

        public EntityFrameworkCoreRepository(IRepositoryContext context)
        {
            DbContext = (DbContext)context.DbContext;
            DbSet = DbContext.Set<TAggregateRoot>();
        }

        #endregion

        #region Repository<TAggregateRoot, TIdentity>

        public override void Create(TAggregateRoot aggregateRoot) => DbSet.Add(aggregateRoot);

        public override void Delete(TAggregateRoot aggregateRoot) => DbSet.Remove(aggregateRoot);

        public override IQueryable<TAggregateRoot> GetAll() => DbSet.AsQueryable();

        public override void Modify(TAggregateRoot aggregateRoot) => DbSet.Update(aggregateRoot);

        public override async Task CreateAsync(TAggregateRoot aggregateRoot)
            => await DbSet.AddAsync(aggregateRoot);

        public override async Task<int> CountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? await GetAll().Where(specification).CountAsync() : await GetAll().CountAsync();

        public override async Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? await GetAll().AnyAsync(specification) : await GetAll().AnyAsync();

        public override async Task<IReadOnlyList<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? await GetAll().Where(specification).ToListAsync() : await GetAll().ToListAsync();

        public override async Task<TAggregateRoot> FindAsync(TIdentity id)
            => await GetAll().FirstOrDefaultAsync(GetIdEqualitySepcification(id));

        public override async Task<TAggregateRoot> FindFirstAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? await GetAll().FirstOrDefaultAsync(specification) : await GetAll().FirstOrDefaultAsync();

        public override async Task<TAggregateRoot> FindSingleAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? await GetAll().SingleOrDefaultAsync(specification) : await GetAll().SingleOrDefaultAsync();

        public override async Task<long> LongCountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? await GetAll().Where(specification).LongCountAsync() : await GetAll().LongCountAsync();

        #endregion

        #region IEntityFrameworkCoreRepository<TAggregateRoot, TIdentity>

        public DbContext DbContext { get; }

        public DbSet<TAggregateRoot> DbSet { get; }

        #endregion
    }
}
