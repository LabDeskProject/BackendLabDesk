using FluentValidation;
using LabDesk.BuildingBlocks.Application.Results;
using MediatR;

using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Application.Behaviors
{
    public class ValidationBehavior<TRequest , TRespone> : IPipelineBehavior<TRequest , TRespone>
        where TRequest : IRequest<TRespone>
        where TRespone : Result

    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TRespone> Handle(
            TRequest request,
            RequestHandlerDelegate<TRespone> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);

            var errors = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .Select(f => new Error(f.PropertyName, f.ErrorMessage))
                .ToList();

            if (errors.Any())
            {
                // Return Result.Failure with list validation without throw Exception
                return (TRespone)(object)Result.Failure(errors.First());
            }

            return await next();
        }
    }
}
