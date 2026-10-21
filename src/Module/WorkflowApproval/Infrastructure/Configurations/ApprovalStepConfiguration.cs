using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Domain;

namespace WorkflowApproval.Infrastructure.Configurations
{
    public sealed class ApprovalStepConfiguration : IEntityTypeConfiguration<ApprovalStep>
    {
        public void Configure(EntityTypeBuilder<ApprovalStep> builder)
        {
            builder.ToTable("ApprovalSteps", "workflow_approval"); builder.HasKey(x => x.Id);
            builder.Property(x => x.Decision).HasConversion<int>().IsRequired();
            builder.Property(x => x.Comment).HasMaxLength(2000);
            builder.HasIndex(x => new { x.WorkflowId, x.StepOrder, x.ApproverUserId }).IsUnique();
        }
    }
}
