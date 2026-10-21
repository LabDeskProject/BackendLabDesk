using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SLA.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Infrastructure.Configurations
{
    public sealed class SlaClockConfiguration : IEntityTypeConfiguration<SlaClock>  
    {
        public void Configure(EntityTypeBuilder<SlaClock> builder)
        {
            builder.ToTable("Clocks", "sla");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.RequestId).IsUnique();
            builder.HasIndex(x => new { x.OrganizationId, x.Status, x.ResolutionDueAtUtc });
        }
    }
}
