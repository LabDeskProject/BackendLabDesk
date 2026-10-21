using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
namespace Notification.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddNotificationApplication(this IServiceCollection services) { services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly)); services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly); services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); return services; }
    private sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) 
        { 
            foreach (var v in validators) await v.ValidateAndThrowAsync(request, cancellationToken); 
            return await next(); 
        }
    }
}
