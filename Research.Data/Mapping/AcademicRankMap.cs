using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a AcademicRank mapping configuration
    /// </summary>
    public partial class AcademicRankMap : ResearchEntityTypeConfiguration<AcademicRank>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<AcademicRank> entity)
        {
            entity.ToTable(nameof(AcademicRank));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NameEn)
                .HasColumnName("NameEN")
                .HasMaxLength(200);

            entity.Property(e => e.NameTh)
                .HasColumnName("NameTH")
                .HasMaxLength(200);
            entity.Ignore(e => e.PersonType);
            base.Configure(entity);
        }

        #endregion
    }
}