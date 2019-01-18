using Microsoft.EntityFrameworkCore;
using Voguedi.Domain.AggregateRoots;

namespace Voguedi.Domain.Repositories.EntityFrameworkCore
{
    public class NoteDbContext : DbContext
    {
        #region Ctors

        public NoteDbContext(DbContextOptions options) : base(options) => Database.EnsureCreated();

        #endregion

        #region Public Properties

        public DbSet<Note> Notes { get; set; }

        #endregion

        #region DbContext

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>().HasKey(k => k.Id);
            modelBuilder.Entity<Note>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<Note>().Property(n => n.Title).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Note>().Property(n => n.Content).IsRequired();
        }

        #endregion
    }
}
