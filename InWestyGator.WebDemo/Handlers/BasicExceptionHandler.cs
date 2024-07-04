using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace InWestyGator.WebDemo.Handlers
{
    public class BasicExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<BasicExceptionHandler> logger;
        public BasicExceptionHandler(ILogger<BasicExceptionHandler> logger)
        {
            this.logger = logger;
        }

        // simple
        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var exceptionMessage = exception.Message;
            logger.LogError(
                "Error Message: {exceptionMessage}, Time of occurrence {time}",
                exceptionMessage, DateTime.UtcNow);

            // false return for continuing with the default middleware behavior
            // true return for signaling that exception is handled and stopping the flow
            // example continues on: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0
            return ValueTask.FromResult(false);
        }
    }
}
