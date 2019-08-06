using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a FiscalSchedule mapping configuration
    /// </summary>
    public partial class FiscalScheduleMap : ResearchEntityTypeConfiguration<FiscalSchedule>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<FiscalSchedule> entity)
        {
            entity.ToTable(nameof(FiscalSchedule));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ClosingDate).HasColumnType("date");

            entity.Property(e => e.FiscalCode).HasMaxLength(10);

            entity.Property(e => e.LastUpdateBy).HasMaxLength(50);

            entity.Property(e => e.OpeningDate).HasColumnType("date");

            base.Configure(entity);
        }

        #endregion
    }
}