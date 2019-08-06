using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Picture mapping configuration
    /// </summary>
    public partial class PictureMap : ResearchEntityTypeConfiguration<Picture>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Picture> entity)
        {
            entity.ToTable(nameof(Picture));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AltAttribute).HasMaxLength(50);

            entity.Property(e => e.MimeType).HasMaxLength(40);

            entity.Property(e => e.SeoFilename).HasMaxLength(300);

            entity.Property(e => e.TitleAttribute).HasMaxLength(20);

            base.Configure(entity);
        }

        #endregion
    }
}