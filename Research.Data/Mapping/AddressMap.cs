using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Address mapping configuration
    /// </summary>
    public partial class AddressMap : ResearchEntityTypeConfiguration<Address>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Address> entity)
        {
            entity.ToTable(nameof(Address));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProvinceId).HasColumnName("ProvinceId");
            entity.Property(e => e.Address1)
                .HasColumnName("Address1")
                .HasMaxLength(400);

            entity.Property(e => e.Address2)
                .HasColumnName("Address2")
                .HasMaxLength(400);

            entity.Property(e => e.ZipCode).HasMaxLength(5);

            entity.HasOne(d => d.Province)
                .WithMany()
                .HasForeignKey(d => d.ProvinceId);

            base.Configure(entity);
        }

        #endregion
    }
}