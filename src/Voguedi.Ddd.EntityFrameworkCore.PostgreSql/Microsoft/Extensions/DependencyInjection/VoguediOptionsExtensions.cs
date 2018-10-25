using System;
using Microsoft.EntityFrameworkCore;
using Voguedi;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VoguediOptionsExtensions
    {
        #region Public Methods

        public static VoguediOptions UsePostgreSqlEntityFrameworkCore<TDbContext>(this VoguediOptions options, string connectionString)
            where TDbContext : DbContext
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            return options.UseEntityFrameworkCore<TDbContext>(s => s.UseNpgsql(connectionString));
        }

        #endregion
    }
}
