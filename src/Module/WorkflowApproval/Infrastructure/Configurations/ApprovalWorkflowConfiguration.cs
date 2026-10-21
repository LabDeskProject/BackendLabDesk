using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Domain;

namespace WorkflowApproval.Infrastructure.Configurations
{
    public sealed class ApprovalWorkflowConfiguration : IEntityTypeConfiguration<ApprovalWorkflow>
    {
        public void Configure(EntityTypeBuilder<ApprovalWorkflow> builder)
        {
            builder.ToTable("ApprovalWorkflows", "workflow_approval"); builder.HasKey(x => x.Id);
            builder.Property(x => x.Mode).HasConversion<int>().IsRequired();
            builder.Property(x => x.RowVersion).IsRowVersion();
            builder.Property(x => x.CreatedAtUtc).IsRequired(); builder.Property(x => x.UpdatedAtUtc).IsRequired();
            builder.HasIndex(x => x.RequestId).IsUnique();
            builder.HasMany(x => x.Steps).WithOne().HasForeignKey(x => x.WorkflowId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
