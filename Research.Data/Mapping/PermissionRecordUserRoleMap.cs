using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Research.Core.Domain.Security;

namespace Research.Data.Mapping.Security
{
    /// <summary>
    /// Represents a permission record-customer role mapping configuration
    /// </summary>
    public partial class PermissionRecordUserRoleMap : ResearchEntityTypeConfiguration<PermissionRecordUserRoleMapping>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<PermissionRecordUserRoleMapping> builder)
        {
            builder.ToTable("PermissionRecord_Role_Mapping");
            builder.HasKey(mapping => new { mapping.PermissionRecordId, mapping.UserRoleId});

            builder.Property(mapping => mapping.PermissionRecordId).HasColumnName("PermissionRecord_Id");
            builder.Property(mapping => mapping.UserRoleId).HasColumnName("UserRole_Id");

            builder.HasOne(mapping => mapping.UserRole)
                .WithMany(role => role.PermissionRecordUserRoleMappings)
                .HasForeignKey(mapping => mapping.UserRoleId)
                .IsRequired();

            builder.HasOne(mapping => mapping.PermissionRecord)
                .WithMany(record => record.PermissionRecordUserRoleMappings)
                .HasForeignKey(mapping => mapping.PermissionRecordId)
                .IsRequired();

            builder.Ignore(mapping => mapping.Id);

            base.Configure(builder);
        }

        #endregion
    }
}