using LabDesk.BuildingBlocks.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LabDesk.BuildingBlocks.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly ICurrentUserService _currentUserService;

        public LoggingBehavior(
            ILogger<LoggingBehavior<TRequest, TResponse>> logger,
            ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId?.ToString() ?? "Anonymous";

            _logger.LogInformation("Processing Request: {RequestName} | User: {UserId}", requestName, userId);

            var stopwatch = Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > 500)
            {
                // Warning if Command/Query run 0.5s (Performance Profiling Mục 23)
                _logger.LogWarning("Long Running Request: {RequestName} ({ElapsedMilliseconds} ms) | User: {UserId}",
                    requestName, stopwatch.ElapsedMilliseconds, userId);
            }
            else
            {
                _logger.LogInformation("Handled Request: {RequestName} in {ElapsedMilliseconds} ms",
                    requestName, stopwatch.ElapsedMilliseconds);
            }

            return response;
        }
    }
}
