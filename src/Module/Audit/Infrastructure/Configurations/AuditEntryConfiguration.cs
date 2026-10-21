using Audit.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Infrastructure.Configurations
{
    public sealed class AuditEntryConfiguration : IEntityTypeConfiguration<AuditEntry>  
    {
        public void Configure(EntityTypeBuilder<AuditEntry> builder)
        {
            builder.ToTable("Entries", "audit");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Action).HasMaxLength(100).IsRequired();
            builder.Property(x => x.EntityType).HasMaxLength(150).IsRequired();
            builder.Property(x => x.EntityId).HasMaxLength(100).IsRequired();
            builder.Property(x => x.CorrelationId).HasMaxLength(100);
            builder.Property(x => x.DetailsJson).HasColumnType("nvarchar(max)");
            builder.HasIndex(x => new { x.OrganizationId, x.EntityType, x.EntityId, x.OccurredAtUtc });
            builder.HasIndex(x => new { x.OrganizationId, x.OccurredAtUtc });
        }
    }
}
