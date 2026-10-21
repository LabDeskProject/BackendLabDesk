using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SLA.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Infrastructure.Configurations
{
    public sealed class SlaPolicyConfiguration : IEntityTypeConfiguration<SlaPolicy>
    {
        public void Configure(EntityTypeBuilder<SlaPolicy> builder)
        {
            builder.ToTable("Policies", "sla"); 
            builder.HasKey(x => x.Id); builder.Property(x => x.RowVersion).IsRowVersion(); 
            builder.HasIndex(x => new { x.OrganizationId, x.ServiceId }).IsUnique().HasFilter("[IsArchived] = 0");
        }
    }
}
