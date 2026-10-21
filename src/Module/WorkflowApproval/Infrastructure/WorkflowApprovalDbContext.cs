using Microsoft.EntityFrameworkCore;
using WorkflowApproval.Domain;

namespace WorkflowApproval.Infrastructure;

public sealed class WorkflowApprovalDbContext(DbContextOptions<WorkflowApprovalDbContext> options) : DbContext(options)
{
    public DbSet<ApprovalWorkflow> Workflows => Set<ApprovalWorkflow>();
    public DbSet<ApprovalStep> Steps => Set<ApprovalStep>();
    public DbSet<ApprovalOutboxMessage> OutboxMessages => Set<ApprovalOutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkflowApprovalDbContext).Assembly);
        modelBuilder.HasDefaultSchema("workflow_approval");
        base.OnModelCreating(modelBuilder);
    }
}
