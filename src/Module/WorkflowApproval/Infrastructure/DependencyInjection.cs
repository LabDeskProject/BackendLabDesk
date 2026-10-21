using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkflowApproval.Application;
using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Infrastructure.Repositories;

namespace WorkflowApproval.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWorkflowApprovalModule(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(cs)) throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        services.AddWorkflowApprovalApplication();
        services.AddHttpContextAccessor();
        services.AddDbContext<WorkflowApprovalDbContext>(o => o.UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(WorkflowApprovalDbContext).Assembly.FullName)));
        services.AddScoped<IApprovalWorkflowRepository, ApprovalWorkflowRepository>();
        services.AddScoped<IApprovalWorkflowQueries, ApprovalWorkflowQueries>();
        services.AddScoped<IApprovalUnitOfWork, ApprovalUnitOfWork>();
        services.AddScoped<IApprovalUserContext, WorkflowApprovalUserContext>();
        return services;
    }
}
