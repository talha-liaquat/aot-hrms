using Aot.Hrms.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aot.Hrms.Api
{
    public class HttpStatusCodeExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpStatusCodeExceptionMiddleware> _logger;

        public HttpStatusCodeExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<HttpStatusCodeExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        private static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void LogAndResponse(HttpContext context, Exception ex, int statusCode)
        {
            var tokenNumber = RandomString(10);

            _logger.LogError(ex, $"Token:{tokenNumber}; HttpStatusCode:{statusCode}; {ex.Message}", tokenNumber);

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = @"text/plain";

            var errorMessage = $"Error: {ex.Message}. Correlation Id: {tokenNumber}";

            //if (statusCode == StatusCodes.Status400BadRequest)
            //    errorMessage += $"\n{ex.Message}";

            context.Response.WriteAsync(errorMessage);
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InsertFailedException ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                LogAndResponse(context, ex, StatusCodes.Status400BadRequest);
                return;
            }
            catch (ValidationException ex) 
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                LogAndResponse(context, ex, StatusCodes.Status400BadRequest);
                return;
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                LogAndResponse(context, ex, StatusCodes.Status500InternalServerError);
                return;
            }
        }
    }
    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpStatusCodeExceptionMiddleware>();
        }
    }
}
