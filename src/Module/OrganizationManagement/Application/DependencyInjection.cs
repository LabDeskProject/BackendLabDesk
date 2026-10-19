using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrganizationManagement.Application.Slices;

namespace OrganizationManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddOrganizationManagementApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(OrganizationManagementValidationBehavior<,>));
        return services;
    }

    private sealed class OrganizationManagementValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            foreach (var validator in validators) await validator.ValidateAndThrowAsync(request, cancellationToken);
            return await next();
        }
    }
}
