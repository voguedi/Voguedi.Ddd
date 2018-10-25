using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Voguedi.Domain.Repositories.EntityFrameworkCore
{
    class EntityFrameworkCoreRepositoryContext<TDbContext> : RepositoryContext<TDbContext>
        where TDbContext : DbContext
    {
        #region Private Fields

        bool disposed = false;

        #endregion

        #region Ctors

        public EntityFrameworkCoreRepositoryContext(TDbContext dbContext) : base(dbContext) { }

        #endregion

        #region RepositoryContext<TDbContext>

        protected override IRepository<TAggregateRoot, TIdentity> CreateRepository<TAggregateRoot, TIdentity>() => new EntityFrameworkCoreRepository<TAggregateRoot, TIdentity>(this);

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    base.Dispose(disposing);
                    DbContext.Dispose();
                }

                disposed = true;
            }
        }

        public override void Commit() => DbContext.SaveChanges();

        public override async Task CommitAsync() => await DbContext.SaveChangesAsync();

        #endregion
    }
}
