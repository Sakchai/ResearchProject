using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a ResearcherHistory mapping configuration
    /// </summary>
    public partial class ResearcherHistoryMap : ResearchEntityTypeConfiguration<ResearcherHistory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ResearcherHistory> entity)
        {
            entity.ToTable(nameof(ResearcherHistory));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TitleName).HasMaxLength(40);
            entity.Property(e => e.FirstName).HasMaxLength(200);

            entity.Property(e => e.LastName).HasMaxLength(200);

            entity.Property(e => e.LastUpdateBy)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Researcher)
                .WithMany()
                .HasForeignKey(d => d.ResearcherId);

            entity.HasOne(d => d.Title)
                .WithMany()
                .HasForeignKey(d => d.TitleId);

            base.Configure(entity);
        }

        #endregion
    }
}