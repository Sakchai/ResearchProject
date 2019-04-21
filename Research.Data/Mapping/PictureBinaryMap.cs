using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a PictureBinary mapping configuration
    /// </summary>
    public partial class PictureBinaryMap : ResearchEntityTypeConfiguration<PictureBinary>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<PictureBinary> entity)
        {
            entity.ToTable(nameof(PictureBinary));
            entity.HasKey(pictureBinary => pictureBinary.Id);

            entity.HasOne(pictureBinary => pictureBinary.Picture)
                .WithOne(picture => picture.PictureBinary)
                .HasForeignKey<PictureBinary>(pictureBinary => pictureBinary.PictureId)
                .OnDelete(DeleteBehavior.Cascade);
            base.Configure(entity);
        }

        #endregion
    }
}