using System;
using System.Net;

namespace QkRest.Contracts
{
    /// <summary>
    /// Exception handler is used to set HTTP status code and return error JSON result to the client.
    /// </summary>
    public interface IQkExceptionHandler
    {
        /// <summary>
        /// Sets HTTP status code and creates JSON results for the client.
        /// </summary>
        /// <param name="exception">Exception to handle.</param>
        /// <param name="code">HTTP status code to be set.</param>
        /// <returns>JSON result for the client.</returns>
        object HandleException(Exception exception, out HttpStatusCode code);
    }
}
