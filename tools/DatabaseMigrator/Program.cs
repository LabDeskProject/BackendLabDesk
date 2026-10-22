using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using OrganizationManagement.Infrastructure;
using ServiceCatalog.Infrastructure;
using RequestManagement.Infrastructure;
using WorkflowApproval.Infrastructure;
using Assignment.Infrastructure;
using SLA.Infrastructure;
using Audit.Infrastructure;
using Notification.Infrastructure;

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("ConnectionStrings__DefaultConnection must be configured.");

await Migrate<IdentityAccessDbContext>("IdentityAccess", context => context.Database.SetCommandTimeout(120));
await Migrate<OrganizationManagementDbContext>("OrganizationManagement");
await Migrate<ServiceCatalogDbContext>("ServiceCatalog");
await Migrate<RequestManagementDbContext>("RequestManagement");
await Migrate<WorkflowApprovalDbContext>("WorkflowApproval");
await Migrate<AssignmentDbContext>("Assignment");
await Migrate<SlaDbContext>("SLA");
await Migrate<AuditDbContext>("Audit");
await Migrate<NotificationDbContext>("Notification");

async Task Migrate<TContext>(string name, Action<TContext>? configure = null) where TContext : DbContext
{
    var options = new DbContextOptionsBuilder<TContext>()
        .UseSqlServer(connectionString, sql => sql.CommandTimeout(120))
        .Options;
    await using var db = (TContext?)Activator.CreateInstance(typeof(TContext), options)
        ?? throw new InvalidOperationException($"Could not construct DbContext {typeof(TContext).Name}.");
    configure?.Invoke(db);
    Console.WriteLine($"Applying migrations: {name}");
    await db.Database.MigrateAsync();
    Console.WriteLine($"Migrations complete: {name}");
}
