using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a ProfessorHistory mapping configuration
    /// </summary>
    public partial class ProfessorHistoryMap : ResearchEntityTypeConfiguration<ProfessorHistory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProfessorHistory> entity)
        {
            entity.ToTable(nameof(ProfessorHistory));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TitleId).HasColumnName("TitleId");
            entity.Property(e => e.TitleName).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(200);
            entity.Property(e => e.LastName).HasMaxLength(200);

            entity.HasOne(d => d.Title)
                .WithMany()
                .HasForeignKey(d => d.TitleId);

            base.Configure(entity);
        }

        #endregion
    }
}