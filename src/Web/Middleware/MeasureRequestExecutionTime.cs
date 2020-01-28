using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Web.Middleware
{
    public class MeasureRequestExecutionTime
    {
        private readonly ILogger<MeasureRequestExecutionTime> _logger;
        private readonly RequestDelegate _next;
        private const string RESPONSE_HEADER_RESPONSE_TIME = "TIME-RESPONSE-IN-MS";

        public MeasureRequestExecutionTime(
            RequestDelegate next,
            ILogger<MeasureRequestExecutionTime> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
           // Start the Timer using Stopwatch  
            var watch = new Stopwatch();  
            watch.Start();  
            httpContext.Response.OnStarting(() => {  
                // Stop the timer information and calculate the time   
                watch.Stop();  
                var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;  
                // Add the Response time information in the Response headers.   
                httpContext.Response.Headers[RESPONSE_HEADER_RESPONSE_TIME] = responseTimeForCompleteRequest.ToString();  
                _logger.LogInformation("REQUEST PROCESSED IN {timespan}MS", responseTimeForCompleteRequest);
                return Task.CompletedTask;  
            });  
            // Call the next delegate/middleware in the pipeline   
            await this._next(httpContext);
        }

    }
}
