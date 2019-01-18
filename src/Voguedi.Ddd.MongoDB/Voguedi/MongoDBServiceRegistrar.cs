using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voguedi.Domain.Repositories;
using Voguedi.Domain.Repositories.MongoDB;
using Voguedi.MongoDB;

namespace Voguedi
{
    public class MongoDBServiceRegistrar<TDbContext> : IServiceRegistrar
        where TDbContext : class, IMongoDBContext
    {
        #region Private Fields

        readonly Action<MongoDBOptions> setupAction;

        #endregion

        #region Ctors

        public MongoDBServiceRegistrar(Action<MongoDBOptions> setupAction) => this.setupAction = setupAction;

        #endregion

        #region IServiceRegistrar

        public void Register(IServiceCollection services)
        {
            services.AddMongoDB<TDbContext>(setupAction);
            services.TryAddScoped<IRepositoryContext, MongoDBRepositoryContext<TDbContext>>();
            services.TryAddScoped<IRepositoryContext<TDbContext>, MongoDBRepositoryContext<TDbContext>>();
        }

        #endregion
    }
}
