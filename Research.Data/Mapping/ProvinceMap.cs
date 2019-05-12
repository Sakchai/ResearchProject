using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Province mapping configuration
    /// </summary>
    public partial class ProvinceMap : ResearchEntityTypeConfiguration<Province>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Province> entity)
        {
            entity.ToTable(nameof(Province));
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(state => state.Abbreviation).HasMaxLength(100);

            base.Configure(entity);
        }

        #endregion
    }
}