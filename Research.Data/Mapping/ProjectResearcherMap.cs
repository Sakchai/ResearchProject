using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a ProjectResearcher mapping configuration
    /// </summary>
    public partial class ProjectResearcherMap : ResearchEntityTypeConfiguration<ProjectResearcher>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProjectResearcher> entity)
        {
            entity.ToTable(nameof(ProjectResearcher));
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.Project)
                .WithMany()
                .HasForeignKey(d => d.ProjectId);

            entity.HasOne(d => d.Researcher)
                .WithMany(researcher => researcher.ProjectResearchers)
                .HasForeignKey(d => d.ResearcherId);

            entity.HasOne(d => d.Title)
                .WithMany()
                .HasForeignKey(d => d.TitleId);
            entity.Ignore(e => e.ProjectRole);
            base.Configure(entity);
        }

        #endregion
    }
}