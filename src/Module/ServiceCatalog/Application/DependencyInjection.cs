using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceCatalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceCatalogApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
    private sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            foreach (var validator in validators) await validator.ValidateAndThrowAsync(request, cancellationToken);
            return await next();
        }
    }
}
