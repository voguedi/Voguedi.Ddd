using System;
using Microsoft.EntityFrameworkCore;
using Voguedi;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VoguediOptionsExtensions
    {
        #region Public Methods

        public static VoguediOptions UseEntityFrameworkCore<TDbContext>(this VoguediOptions options, Action<DbContextOptionsBuilder> setupAction)
            where TDbContext : DbContext
        {
            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));

            options.Register(new EntityFrameworkCoreServiceRegistrar<TDbContext>(setupAction));
            return options;
        }

        #endregion
    }
}
