using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a User mapping configuration
    /// </summary>
    public partial class UserMap : ResearchEntityTypeConfiguration<User>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable(nameof(User));
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Email).HasMaxLength(200);

            entity.Property(e => e.Description)
                .HasMaxLength(200);

            entity.Property(e => e.LastUpdateBy)
                .HasMaxLength(50);
            entity.Property(e => e.MobileNumber).HasMaxLength(10);

            entity.Property(e => e.Password).HasMaxLength(50);

            entity.Property(e => e.SessionId).HasMaxLength(500);

            entity.Ignore(e => e.UserType);

            entity.HasOne(d => d.Agency)
                .WithMany()
                .HasForeignKey(d => d.AgencyId);

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