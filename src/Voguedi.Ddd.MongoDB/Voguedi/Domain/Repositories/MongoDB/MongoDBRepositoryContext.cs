using System.Threading.Tasks;
using Voguedi.MongoDB;

namespace Voguedi.Domain.Repositories.MongoDB
{
    class MongoDBRepositoryContext<TDbContext> : RepositoryContext<TDbContext>
        where TDbContext : class, IMongoDBContext
    {
        #region Ctors

        public MongoDBRepositoryContext(TDbContext dbContext) : base(dbContext) { }

        #endregion

        #region RepositoryContext<TMongoDBContext>

        protected override IRepository<TAggregateRoot, TIdentity> CreateRepository<TAggregateRoot, TIdentity>() => new MongoDBRepository<TAggregateRoot, TIdentity>(this);

        public override void Commit() => DbContext.SaveChanges();

        public override async Task CommitAsync() => await DbContext.SaveChangesAsync();

        #endregion
    }
}
