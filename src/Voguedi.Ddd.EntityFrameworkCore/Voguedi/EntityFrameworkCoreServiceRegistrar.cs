using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voguedi.Domain.Repositories;
using Voguedi.Domain.Repositories.EntityFrameworkCore;

namespace Voguedi
{
    public class EntityFrameworkCoreServiceRegistrar<TDbContext> : IServiceRegistrar
        where TDbContext : DbContext
    {
        #region Private Fields

        readonly Action<DbContextOptionsBuilder> setupAction;

        #endregion

        #region Ctors

        public EntityFrameworkCoreServiceRegistrar(Action<DbContextOptionsBuilder> setupAction) => this.setupAction = setupAction;

        #endregion

        #region IServiceRegistrar

        public void Register(IServiceCollection services)
        {
            services.AddDbContextPool<TDbContext>(setupAction);
            services.TryAddScoped<IRepositoryContext, EntityFrameworkCoreRepositoryContext<TDbContext>>();
            services.TryAddScoped<IRepositoryContext<TDbContext>, EntityFrameworkCoreRepositoryContext<TDbContext>>();
        }

        #endregion
    }
}
