using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a ProjectProgress mapping configuration
    /// </summary>
    public partial class ProjectProgressMap : ResearchEntityTypeConfiguration<ProjectProgress>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProjectProgress> entity)
        {
            entity.ToTable(nameof(ProjectProgress));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LastUpdateBy).HasMaxLength(50);

            entity.Property(e => e.Comment).HasMaxLength(2000);

            entity.Property(e => e.ProgressEndDate).HasColumnType("date");

            entity.Property(e => e.ProgressStartDate).HasColumnType("date");

            entity.HasOne(d => d.Project)
                .WithMany(project => project.ProjectProgresses)
                .HasForeignKey(d => d.ProjectId);

            entity.Ignore(e => e.ProgressStatus);
            base.Configure(entity);
        }

        #endregion
    }
}