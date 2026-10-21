using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Domain;

namespace WorkflowApproval.Infrastructure.Configurations
{
    public sealed class ApprovalOutboxMessageConfiguration : IEntityTypeConfiguration<ApprovalOutboxMessage>
    {
        public void Configure(EntityTypeBuilder<ApprovalOutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages", "workflow_approval");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Type).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Payload).HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(x => x.LastError).HasMaxLength(1000);
            builder.HasIndex(x => new { x.ProcessedAtUtc, x.ClaimedAtUtc, x.OccurredAtUtc });
        }
    }
}
