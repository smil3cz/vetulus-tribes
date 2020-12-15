using GreenFoxAcademy.SpaceSettlers.Database;
using GreenFoxAcademy.SpaceSettlers.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Logging
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggingMiddleware
    {
        private readonly RequestDelegate next;
        private int? LogId { get; set; }
        private string? Env { get => AppSettings.EnvironmentVariable; }

        public LoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await RequestLog(httpContext);

            await ResponseLog(httpContext);
        }

        private async Task RequestLog(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            var logData = new LogData()
            {
                Level = AppSettings.EnvironmentVariable == "Development" ? "DEBUG" : "INFO",
                Time = DateTime.UtcNow
            };

            logData.Request.Body = String.Empty;
            if (httpContext.Request.Body.CanSeek)
            {
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                var reqStream = new StreamReader(httpContext.Request.Body);
                logData.Request.Body = await reqStream.ReadToEndAsync();
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            logData.Request.Path = httpContext.Request.Path.ToString();
            logData.Request.Endpoint = httpContext.GetEndpoint()?.DisplayName;
            logData.Request.TraceIdentifier = httpContext.TraceIdentifier;

            LogId = await Log(logData, null, httpContext);
        }

        private async Task ResponseLog(HttpContext httpContext)
        {
            var originalBodyStream = httpContext.Response.Body;
            httpContext.Response.Body = new MemoryStream();

            var logData = new LogData()
            {
                Level = AppSettings.EnvironmentVariable == "Development" ? "DEBUG" : "INFO",
                Time = DateTime.UtcNow
            };

            try
            {
                await next(httpContext);
            }
            catch (Exception e)
            {
                logData.Level = "ERROR";
                httpContext.Response.StatusCode = 418;
                byte[] error = Encoding.UTF8.GetBytes(e.Message);
                httpContext.Response.Body = new MemoryStream(error);
            }

            logData.Response.Body = String.Empty;
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var resStream = new StreamReader(httpContext.Response.Body);
            logData.Response.Body = await resStream.ReadToEndAsync();
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

            logData.Response.StatusCode = httpContext.Response.StatusCode;

            LogId = await Log(logData, LogId, httpContext);

            await httpContext.Response.Body.CopyToAsync(originalBodyStream);
        }

        private async Task<int?> Log(LogData logData, int? logId, HttpContext httpContext)
        {
            switch (Env)
            {
                case "Development":
                    var loggerF = new FileLogger();
                    return await loggerF.Log(logData, logId);

                default:
                    var dbContext = (ApplicationDbContext)httpContext.RequestServices.GetService(typeof(ApplicationDbContext));
                    var loggerDb = new DBLogger(dbContext);
                    return await loggerDb.Log(logData, logId);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
