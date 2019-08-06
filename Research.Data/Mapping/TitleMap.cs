using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Title mapping configuration
    /// </summary>
    public partial class TitleMap : ResearchEntityTypeConfiguration<Title>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Title> entity)
        {
            entity.ToTable(nameof(Title));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TitleNameEN)
                .HasMaxLength(100);
            entity.Property(e => e.TitleNameTH)
                .HasMaxLength(100);
            entity.Property(e => e.Gender)
                .HasMaxLength(1);
            base.Configure(entity);
        }

        #endregion
    }
}