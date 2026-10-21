using Assignment.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Infrastructure.Configurations
{
    public sealed class AssignmentRecordConfiguration : IEntityTypeConfiguration<AssignmentRecord>
    {
        public void Configure(EntityTypeBuilder<AssignmentRecord> builder)
        {
            builder.ToTable("Assignments", "assignment"); builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).HasConversion<int>().IsRequired();
            builder.Property(x => x.Note).HasMaxLength(2000);
            builder.Property(x => x.AssignedAtUtc).IsRequired();
            builder.HasIndex(x => x.RequestId).IsUnique().HasFilter("[Status] = 1");
            builder.HasIndex(x => new { x.OrganizationId, x.AssignedToUserId, x.Status, x.AssignedAtUtc });   
        }
    }
}
