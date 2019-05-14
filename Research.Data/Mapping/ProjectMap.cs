using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Project mapping configuration
    /// </summary>
    public partial class ProjectMap : ResearchEntityTypeConfiguration<Project>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Project> entity)
        {
            entity.ToTable(nameof(Project));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProjectEndDate).HasColumnType("date");

            entity.Property(e => e.FundAmount).HasColumnType("numeric(18, 2)");

            entity.Property(e => e.LastUpdateBy).HasMaxLength(50);

            entity.Property(e => e.ProjectCode).HasMaxLength(50);

            entity.Property(e => e.ProjectNameEn)
                .HasColumnName("ProjectNameEN")
                .HasMaxLength(500);

            entity.Property(e => e.ProjectNameTh)
                .HasColumnName("ProjectNameTH")
                .HasMaxLength(500);

            entity.Property(e => e.PlanNameEn)
                .HasColumnName("PlanNameEN")
                .HasMaxLength(500);

            entity.Property(e => e.PlanNameTh)
                .HasColumnName("PlanNameTH")
                .HasMaxLength(500);

            entity.Property(e => e.ProjectType)
                .HasColumnName("ProjectType")
                .HasMaxLength(1);

            entity.Property(e => e.ProjectStartDate).HasColumnType("date");

            entity.Property(e => e.DownloadId).HasColumnName("DownloadId");
            entity.Property(e => e.InternalProfessorId).HasColumnName("InternalProfessorId");
            entity.Property(e => e.InternalProfessor2Id).HasColumnName("InternalProfessor2Id");
            entity.Property(e => e.ExternalProfessorId).HasColumnName("ExternalProfessorId");
            entity.Property(e => e.StrategyGroupId).HasColumnName("StrategyGroupId");
            entity.Property(e => e.FiscalScheduleId).HasColumnName("FiscalScheduleId");
            entity.Property(e => e.ProjectStatusId).HasColumnName("ProjectStatusId");
            

            entity.HasOne(e => e.InternalProfessor)
                .WithMany()
                .HasForeignKey(e => e.InternalProfessorId);

            entity.HasOne(e => e.InternalProfessor2)
                .WithMany()
                .HasForeignKey(e => e.InternalProfessor2Id);

            entity.HasOne(e => e.ExternalProfessor)
                .WithMany()
                .HasForeignKey(e => e.ExternalProfessorId);

            entity.HasOne(e => e.StrategyGroup)
                .WithMany()
                .HasForeignKey(e => e.StrategyGroupId);


            entity.HasOne(e => e.FiscalSchedule)
                .WithMany()
                .HasForeignKey(e => e.FiscalScheduleId);

            entity.Ignore(e => e.ProjectStatus);
            entity.Ignore(e => e.ProjectProgresses);
            entity.Ignore(e => e.ProjectResearchers);
            entity.Ignore(e => e.ProjectHistories);
            base.Configure(entity);
        }

        #endregion
    }
}