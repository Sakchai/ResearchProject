using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Professor mapping configuration
    /// </summary>
    public partial class ProfessorMap : ResearchEntityTypeConfiguration<Professor>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Professor> entity)
        {
            entity.ToTable(nameof(Professor));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.TitleName).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(200);
            entity.Property(e => e.LastName).HasMaxLength(200);
            entity.Property(e => e.Telephone).HasMaxLength(10);
            entity.Property(e => e.ProfessorCode).HasMaxLength(10);
            entity.Property(e => e.Comment).HasMaxLength(300);

            entity.HasOne(d => d.Address)
                .WithMany()
                .HasForeignKey(d => d.AddressId);

            entity.HasOne(d => d.Title)
                .WithMany()
                .HasForeignKey(d => d.TitleId);

            entity.Ignore(e => e.ProfessorType);
            entity.Ignore(e => e.ProfessorHistories);
            base.Configure(entity);
        }

        #endregion
    }
}