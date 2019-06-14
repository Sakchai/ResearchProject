using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Research.Core.Domain.Common;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a generic attribute mapping configuration
    /// </summary>
    public partial class GenericAttributeMap : ResearchEntityTypeConfiguration<GenericAttribute>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<GenericAttribute> builder)
        {
            builder.ToTable(nameof(GenericAttribute));
            builder.HasKey(attribute => attribute.Id);

            builder.Property(attribute => attribute.KeyGroup).HasMaxLength(400).IsRequired();
            builder.Property(attribute => attribute.Key).HasMaxLength(400).IsRequired();
            builder.Property(attribute => attribute.Value).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}