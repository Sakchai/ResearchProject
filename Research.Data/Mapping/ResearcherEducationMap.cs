﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a ResearcherEducation mapping configuration
    /// </summary>
    public partial class ResearcherEducationMap : ResearchEntityTypeConfiguration<ResearcherEducation>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ResearcherEducation> entity)
        {
            entity.ToTable(nameof(ResearcherEducation));
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Subject)
                .HasMaxLength(100);

            entity.HasOne(d => d.Country)
                .WithMany()
                .HasForeignKey(d => d.CountryId);

            entity.HasOne(d => d.EducationLevel)
                .WithMany()
                .HasForeignKey(d => d.EducationLevelId);

            entity.HasOne(d => d.Institute)
                .WithMany()
                .HasForeignKey(d => d.InstituteId);

            entity.HasOne(d => d.Researcher)
                .WithMany()
                .HasForeignKey(d => d.ResearcherId);

            base.Configure(entity);
        }

        #endregion
    }
}