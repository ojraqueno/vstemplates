using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Core.Web.Infrastructure
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IHostingEnvironment hostingEnvironment, IModelMetadataProvider modelMetadataProvider, ILogger<ErrorHandlingMiddleware> logger)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            logger.LogInformation("test error");

            //if (exception is MyNotFoundException) code = HttpStatusCode.NotFound;
            //else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (exception is MyException) code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}