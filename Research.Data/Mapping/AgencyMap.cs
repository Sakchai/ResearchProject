using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Agency mapping configuration
    /// </summary>
    public partial class AgencyMap : ResearchEntityTypeConfiguration<Agency>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Agency> entity)
        {
            entity.ToTable(nameof(Agency));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .HasMaxLength(100);

            base.Configure(entity);
        }

        #endregion
    }
}