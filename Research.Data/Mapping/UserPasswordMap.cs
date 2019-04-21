using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a UserPassword mapping configuration
    /// </summary>
    public partial class UserPasswordMap : ResearchEntityTypeConfiguration<UserPassword>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<UserPassword> entity)
        {
            entity.ToTable(nameof(UserPassword));
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Password);

            base.Configure(entity);
        }

        #endregion
    }
}