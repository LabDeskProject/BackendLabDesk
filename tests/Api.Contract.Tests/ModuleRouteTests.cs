using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OrganizationManagement.API.Controllers;
using ServiceCatalog.API.Controllers;
using RequestManagement.API.Controllers;
using WorkflowApproval.API.Controllers;
using Assignment.API.Controllers;
using SLA.API.Controllers;
using Audit.API.Controllers;
using Notification.API.Controllers;
using System.Reflection;

namespace Api.Contract.Tests;

public sealed class ModuleRouteTests
{
    private static readonly Type[] Controllers = [typeof(OrganizationManagementController), typeof(MembershipController), typeof(OrganizationManagement.API.Controllers.ServicesController), typeof(ServiceCatalogController), typeof(RequestsController), typeof(ApprovalWorkflowsController), typeof(AssignmentsController), typeof(SlaController), typeof(AuditController), typeof(NotificationsController)];

    [Fact]
    public void CurrentModuleControllerRoutesAreUnique()
    {
        var entries = Controllers.Select(t => (Controller: t, Route: t.GetCustomAttribute<RouteAttribute>()?.Template)).ToArray();
        Assert.All(entries, e => Assert.False(string.IsNullOrWhiteSpace(e.Route), $"{e.Controller.Name} lacks a route"));
        Assert.Equal(entries.Length, entries.Select(e => e.Route).Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    [Fact]
    public void EveryCurrentModuleControllerRequiresAuthenticationByDefault()
    {
        Assert.All(Controllers, controller => Assert.NotNull(controller.GetCustomAttribute<AuthorizeAttribute>()));
    }
}
