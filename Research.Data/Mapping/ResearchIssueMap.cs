using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a ResearchIssue mapping configuration
    /// </summary>
    public partial class ResearchIssueMap : ResearchEntityTypeConfiguration<ResearchIssue>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ResearchIssue> entity)
        {
            entity.ToTable(nameof(ResearchIssue));
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IssueCode)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.LastUpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                .HasMaxLength(100);
            base.Configure(entity);
        }

        #endregion
    }
}