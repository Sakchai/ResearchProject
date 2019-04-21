using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a ProjectHistory mapping configuration
    /// </summary>
    public partial class ProjectHistoryMap : ResearchEntityTypeConfiguration<ProjectHistory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProjectHistory> entity)
        {
            entity.ToTable(nameof(ProjectHistory));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LastUpdateBy).HasMaxLength(50);

            entity.Property(e => e.ProjectNameTh)
                .HasColumnName("ProjectNameTH")
                .HasMaxLength(500);

            entity.HasOne(d => d.Project)
                .WithMany()
                .HasForeignKey(d => d.ProjectId);

            base.Configure(entity);
        }

        #endregion
    }
}