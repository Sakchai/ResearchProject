using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a ProjectProfessor mapping configuration
    /// </summary>
    public partial class ProjectProfessorMap : ResearchEntityTypeConfiguration<ProjectProfessor>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProjectProfessor> entity)
        {
            entity.ToTable(nameof(ProjectProfessor));
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.Project)
                .WithMany(project => project.ProjectProfessors)
                .HasForeignKey(d => d.ProjectId);
            entity.Property(d => d.ProfessorName).HasMaxLength(200);
            entity.Ignore(d => d.ProfessorType);
            base.Configure(entity);
        }

        #endregion
    }
}