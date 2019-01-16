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

        public override Task CreateAsync(TAggregateRoot aggregateRoot) => DbSet.AddAsync(aggregateRoot);

        public override async Task DeleteAsync(TIdentity id) => await DeleteAsync(await FindAsync(id));

        public override async Task DeleteAsync(Expression<Func<TAggregateRoot, bool>> specification)
        {
            foreach (var aggregateRoot in await FindAllAsync(specification))
                await DeleteAsync(aggregateRoot);
        }

        public override Task<int> CountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? GetAll().Where(specification).CountAsync() : GetAll().CountAsync();

        public override Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? GetAll().AnyAsync(specification) : GetAll().AnyAsync();

        public override async Task<IReadOnlyList<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? await GetAll().Where(specification).ToListAsync() : await GetAll().ToListAsync();

        public override Task<TAggregateRoot> FindAsync(TIdentity id) => GetAll().FirstOrDefaultAsync(GetIdEqualitySepcification(id));

        public override Task<TAggregateRoot> FindFirstAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? GetAll().FirstOrDefaultAsync(specification) : GetAll().FirstOrDefaultAsync();

        public override Task<TAggregateRoot> FindSingleAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? GetAll().SingleOrDefaultAsync(specification) : GetAll().SingleOrDefaultAsync();

        public override Task<long> LongCountAllAsync(Expression<Func<TAggregateRoot, bool>> specification = null)
            => specification != null ? GetAll().Where(specification).LongCountAsync() : GetAll().LongCountAsync();

        #endregion

        #region IEntityFrameworkCoreRepository<TAggregateRoot, TIdentity>

        public DbContext DbContext { get; }

        public DbSet<TAggregateRoot> DbSet { get; }

        #endregion
    }
}
