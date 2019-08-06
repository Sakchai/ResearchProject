using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a UserRole mapping configuration
    /// </summary>
    public partial class UserRoleMap : ResearchEntityTypeConfiguration<UserRole>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<UserRole> entity)
        {
            entity.ToTable(nameof(UserRole));
            entity.HasKey(role => role.Id);
            entity.Property(role => role.Name).HasMaxLength(255).IsRequired();
            entity.Property(role => role.SystemName).HasMaxLength(255);
            base.Configure(entity);
        }

        #endregion
    }
}