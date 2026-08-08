using FluentValidation;
using LabDesk.BuildingBlocks.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LabDesk.BuildingBlocks.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        Assembly assembly)
    {
        // Register MediatR and Handlers (DLL)
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);

            // Register Pipeline Behavior validate
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Find and register all FluentValidation Validators
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
