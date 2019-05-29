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
            entity.HasKey(d => new { d.RoleId, d.UserId });
            entity.HasOne(d => d.Role)
                .WithMany()
                .HasForeignKey(d => d.RoleId);

            entity.HasOne(d => d.User)
                .WithMany(user => user.UserRoles)
                .HasForeignKey(d => d.UserId);

            entity.Ignore(e => e.Id);
            entity.Ignore(d => d.RolePrograms);
            base.Configure(entity);
        }

        #endregion
    }
}