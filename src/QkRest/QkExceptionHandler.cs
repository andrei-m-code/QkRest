using System;
using System.Net;
using QkRest.Contracts;
using QkRest.Exceptions;

namespace QkRest
{
    /// <summary>
    /// Exception handler is used to set HTTP status code and return error JSON result to the client.
    /// </summary>
    public class QkExceptionHandler : IQkExceptionHandler
    {
        /// <summary>
        /// Sets HTTP status code and creates JSON results for the client.
        /// </summary>
        /// <param name="exception">Exception to handle.</param>
        /// <param name="code">HTTP status code to be set.</param>
        /// <returns>JSON result for the client.</returns>
        public virtual object HandleException(Exception exception, out HttpStatusCode code)
        {
            code = HttpStatusCode.InternalServerError;

            if (exception is QkNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is QkUnauthorizedException) code = HttpStatusCode.Unauthorized;
            else if (exception is QkException) code = HttpStatusCode.BadRequest;

            return new QkResponse(exception);
        }
    }
}
