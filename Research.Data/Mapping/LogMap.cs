using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Log mapping configuration
    /// </summary>
    public partial class LogMap : ResearchEntityTypeConfiguration<Log>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Log> entity)
        {
            entity.ToTable(nameof(Log));
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ShortMessage).IsRequired();
            entity.Property(e => e.IpAddress).HasMaxLength(200);

            entity.Ignore(e => e.LogLevel);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);

            base.Configure(entity);
        }

        #endregion
    }
}