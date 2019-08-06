using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Download mapping configuration
    /// </summary>
    public partial class DownloadMap : ResearchEntityTypeConfiguration<Download>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Download> entity)
        {
            entity.ToTable(nameof(Download));
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ContentType).HasMaxLength(50);

            entity.Property(e => e.DownloadUrl).HasMaxLength(256);

            entity.Property(e => e.Extension).HasMaxLength(10);

            entity.Property(e => e.Filename).HasMaxLength(256);

            base.Configure(entity);
        }

        #endregion
    }
}