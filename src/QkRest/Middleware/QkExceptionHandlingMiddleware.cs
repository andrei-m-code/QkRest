using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QkRest.Helpers;
using QkRest.Contracts;

namespace QkRest.Middleware
{
    /// <summary>
    /// Middleware that catches and processes exceptions that occur during HTTP request processing.
    /// </summary>
    public class QkExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IQkExceptionHandler exceptionHandler;

        /// <summary>
        /// Constructor.
        /// </summary>
        public QkExceptionHandlingMiddleware(RequestDelegate next, IQkExceptionHandler exceptionHandler)
        {
            this.next = next;
            this.exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Execute middleware.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handle exceptions that occur during ASP.NET Core request processing.
        /// </summary>
        protected virtual Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var body = exceptionHandler.HandleException(exception, out HttpStatusCode code);

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;
            return response.WriteAsync(JsonHelper.SerializeObject(body));
        }
    }
}
