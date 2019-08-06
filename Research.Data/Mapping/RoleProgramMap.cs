using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a RoleProgram mapping configuration
    /// </summary>
    public partial class RoleProgramMap : ResearchEntityTypeConfiguration<RoleProgram>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<RoleProgram> entity)
        {
            entity.ToTable(nameof(RoleProgram));
            entity.HasKey(e => e.Id);

  
            entity.Property(e => e.LastUpdateBy)
                .HasMaxLength(50);

            entity.HasOne(d => d.Program)
                .WithMany(role => role.RolePrograms)
                .HasForeignKey(d => d.ProgramId);

            entity.HasOne(d => d.Role)
                .WithMany()
                .HasForeignKey(d => d.RoleId);

            base.Configure(entity);
        }

        #endregion
    }
}