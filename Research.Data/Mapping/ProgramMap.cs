using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Program mapping configuration
    /// </summary>
    public partial class ProgramMap : ResearchEntityTypeConfiguration<Program>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Program> entity)
        {
            entity.ToTable(nameof(Program));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LastUpdateBy).HasMaxLength(50);

            entity.Property(e => e.ProgramCode).HasMaxLength(10);

            entity.Property(e => e.ProgramName).HasMaxLength(50);

            entity.Property(e => e.ProgramUrl)
                .HasColumnName("ProgramURL")
                .HasMaxLength(300);

            base.Configure(entity);
        }

        #endregion
    }
}