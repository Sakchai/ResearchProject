﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Research.Data.Mapping
{
    /// <summary>
    /// Represents a Faculty mapping configuration
    /// </summary>
    public partial class FacultyMap : ResearchEntityTypeConfiguration<Faculty>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="entity">The entity to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Faculty> entity)
        {
            entity.ToTable(nameof(Faculty));
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(100);

            base.Configure(entity);
        }

        #endregion
    }
}