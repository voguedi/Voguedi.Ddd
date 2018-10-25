using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Domain.Model;

namespace Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Domain.Repositories.ModelTypeConfigurations
{
    public class NoteTypeConfiguration : IEntityTypeConfiguration<Note>
    {
        #region IEntityTypeConfiguration<Note>

        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Id).IsRequired().HasMaxLength(24);
            builder.Property(n => n.Title).IsRequired().HasMaxLength(64);
            builder.Property(n => n.Content).IsRequired();
        }

        #endregion
    }
}
