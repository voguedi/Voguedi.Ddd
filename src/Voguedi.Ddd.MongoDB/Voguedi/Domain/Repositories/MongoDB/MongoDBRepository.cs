using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Voguedi.Domain.AggregateRoots;
using Voguedi.MongoDB;

namespace Voguedi.Domain.Repositories.MongoDB
{
    class MongoDBRepository<TAggregateRoot, TIdentity> : Repository<TAggregateRoot, TIdentity>, IMongoDBRepository<TAggregateRoot, TIdentity>
        where TAggregateRoot : class, IAggregateRoot<TIdentity>
    {
        #region Ctors

        public MongoDBRepository(IRepositoryContext context)
        {
            DbContext = (IMongoDBContext)context.DbContext;
            Database = DbContext.Database;
            Collection = Database.GetCollection<TAggregateRoot>(typeof(TAggregateRoot).Name);
            Session = DbContext.Session;
        }

        #endregion

        #region Private Fields

        static FilterDefinition<TAggregateRoot> BuildSpecification(TAggregateRoot aggregateRoot) => Builders<TAggregateRoot>.Filter.Eq(agg => agg.Id, aggregateRoot.Id);

        static FilterDefinition<TAggregateRoot> BuildSpecification(TIdentity id) => Builders<TAggregateRoot>.Filter.Eq(agg => agg.Id, id);

        #endregion

        #region Repository<TAggregateRoot, TIdentity>

        public override void Create(TAggregateRoot aggregateRoot) => Collection.InsertOne(Session, aggregateRoot);

        public override void Delete(TAggregateRoot aggregateRoot) => Collection.DeleteOne(Session, BuildSpecification(aggregateRoot));

        public override void Delete(TIdentity id) => Collection.DeleteOne(Session, BuildSpecification(id));

        public override void Modify(TAggregateRoot aggregateRoot) => Collection.ReplaceOne(Session, BuildSpecification(aggregateRoot), aggregateRoot);

        public override Task CreateAsync(TAggregateRoot aggregateRoot) => Collection.InsertOneAsync(Session, aggregateRoot);

        public override Task DeleteAsync(TAggregateRoot aggregateRoot) => Collection.DeleteOneAsync(Session, BuildSpecification(aggregateRoot));

        public override Task DeleteAsync(TIdentity id) => Collection.DeleteOneAsync(Session, BuildSpecification(id));

        public override Task ModifyAsync(TAggregateRoot aggregateRoot) => Collection.ReplaceOneAsync(Session, BuildSpecification(aggregateRoot), aggregateRoot);

        public override IQueryable<TAggregateRoot> GetAll() => Collection.AsQueryable();

        public override TAggregateRoot Find(TIdentity id) => Collection.Find(BuildSpecification(id)).FirstOrDefault();

        public override Task<TAggregateRoot> FindAsync(TIdentity id) => Collection.Find(BuildSpecification(id)).FirstOrDefaultAsync();

        #endregion

        #region IMongoDBRepository<TAggregateRoot, TIdentity>

        public IMongoDBContext DbContext { get; }

        public IMongoDatabase Database { get; }

        public IMongoCollection<TAggregateRoot> Collection { get; }

        public IClientSessionHandle Session { get; }

        #endregion
    }
}
