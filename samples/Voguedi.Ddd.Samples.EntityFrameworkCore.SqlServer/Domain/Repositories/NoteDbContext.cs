using Microsoft.EntityFrameworkCore;
using Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Domain.Model;
using Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Domain.Repositories.ModelTypeConfigurations;

namespace Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Domain.Repositories
{
    public class NoteDbContext : DbContext
    {
        #region Public Properties

        public DbSet<Note> Notes { get; set; }

        #endregion

        #region Ctors

        public NoteDbContext(DbContextOptions options) : base(options) => Database.EnsureCreated();

        #endregion

        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfiguration(new NoteTypeConfiguration());

        #endregion
    }
}
