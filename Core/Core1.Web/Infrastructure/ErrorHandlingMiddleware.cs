using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Core1.Web.Infrastructure
{
    // More info: https://stackoverflow.com/questions/38630076/asp-net-core-web-api-exception-handling
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IHostingEnvironment hostingEnvironment, IModelMetadataProvider modelMetadataProvider, ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, hostingEnvironment, modelMetadataProvider, logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, IHostingEnvironment hostingEnvironment, IModelMetadataProvider modelMetadataProvider, ILogger<ErrorHandlingMiddleware> logger)
        {
            await Task.Run(() =>
            {
                logger.LogError(exception, exception.InnermostException().Message);
            });
        }
    }
}