using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Role mapping configuration
    /// </summary>
    public partial class RoleMap : ResearchEntityTypeConfiguration<Role>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.ToTable(nameof(Role));
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.LastUpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.RoleName)
                .HasMaxLength(100);
            entity.Property(e => e.RoleDesc)
                .HasMaxLength(200);
            entity.Ignore(e => e.RolePrograms);
            entity.Ignore(e => e.UserRoles);

            base.Configure(entity);
        }

        #endregion
    }
}