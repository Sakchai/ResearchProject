using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Country mapping configuration
    /// </summary>
    public partial class CountryMap : ResearchEntityTypeConfiguration<Country>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Country> entity)
        {
            entity.ToTable(nameof(Country));
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(100);
            entity.Ignore(e => e.Provinces);
            entity.Ignore(e => e.ResearcherEducations);
            base.Configure(entity);
        }

        #endregion
    }
}