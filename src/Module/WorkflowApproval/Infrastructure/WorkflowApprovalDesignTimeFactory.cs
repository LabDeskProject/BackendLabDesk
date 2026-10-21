using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorkflowApproval.Infrastructure;

public sealed class WorkflowApprovalDesignTimeFactory : IDesignTimeDbContextFactory<WorkflowApprovalDbContext>
{
    public WorkflowApprovalDbContext CreateDbContext(string[] args)
    {
        var cs = Environment.GetEnvironmentVariable("LABDESK_CONNECTION_STRING") ?? "Server=(localdb)\\MSSQLLocalDB;Database=LabDeskMigrationDesign;Trusted_Connection=True;TrustServerCertificate=True";
        return new WorkflowApprovalDbContext(new DbContextOptionsBuilder<WorkflowApprovalDbContext>().UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(WorkflowApprovalDbContext).Assembly.FullName)).Options);
    }
}
