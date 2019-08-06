using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Researcher mapping configuration
    /// </summary>
    public partial class ResearcherMap : ResearchEntityTypeConfiguration<Researcher>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Researcher> entity)
        {
            entity.ToTable(nameof(Researcher));
            entity.HasKey(e => e.Id);

            entity.Property(e => e.AcademicRankId).HasColumnName("AcademicRankId");
            entity.Property(e => e.AddressId).HasColumnName("AddressId");
            entity.Property(e => e.AgencyId).HasColumnName("AgencyId");
            entity.Property(e => e.PictureId).HasColumnName("PictureId");
            entity.Property(e => e.TitleId).HasColumnName("TitleId");

            entity.Property(e => e.Email).HasMaxLength(1000);
            entity.Property(e => e.TitleName).HasMaxLength(40);

            entity.Property(e => e.FirstName).HasMaxLength(200);

            entity.Property(e => e.LastName).HasMaxLength(200);
            entity.Property(e => e.FirstNameEN).HasMaxLength(200);

            entity.Property(e => e.LastNameEN).HasMaxLength(200);
            entity.Property(e => e.Gender).HasColumnName("Gender")
                .HasMaxLength(1);

            entity.Property(e => e.IDCard).HasColumnName("IDCard")
                .HasMaxLength(13);

            entity.Property(e => e.LastName).HasMaxLength(200);

            entity.Property(e => e.LastUpdateBy).HasMaxLength(50);

            entity.Property(e => e.Telephone).HasMaxLength(10);

            entity.Property(e => e.ResearcherCode)
                .HasMaxLength(50);

            entity.HasOne(d => d.Picture)
                .WithMany()
                .HasForeignKey(d => d.PictureId);

            entity.HasOne(d => d.Agency)
                .WithMany()
                .HasForeignKey(d => d.AgencyId);

            entity.HasOne(d => d.Title)
                .WithMany()
                .HasForeignKey(d => d.TitleId);

            entity.HasOne(d => d.Address)
                .WithMany()
                .HasForeignKey(d => d.AddressId);
            entity.Property(e => e.Birthdate).HasColumnType("date");

            entity.Ignore(e => e.PersonalType);
            entity.Ignore(e => e.ProjectResearchers);
            entity.Ignore(e => e.ResearcherEducations);
            entity.Ignore(e => e.ResearcherHistories);
            entity.Ignore(e => e.Users);

            base.Configure(entity);
        }

        #endregion
    }
}