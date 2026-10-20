using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCatalog.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCatalog.Infrastructure.Configuration
{
    public sealed class ServiceDefinitionConfiguration : IEntityTypeConfiguration<ServiceDefinition>
    {
        public void Configure(EntityTypeBuilder<ServiceDefinition> builder)
        {
            builder.ToTable("Services", "service_catalog");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.CreatedAtUtc).IsRequired();
            builder.Property(x => x.UpdatedAtUtc).IsRequired();
            builder.Property(x => x.IsArchived).IsRequired();
            builder.HasIndex(x => new { x.OrganizationId, x.Name }).IsUnique().HasFilter("[IsArchived] = 0");
        }
    }
}
