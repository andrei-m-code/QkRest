using System;
using System.Net;
using QkRest.Contracts;
using QkRest.Exceptions;

namespace QkRest
{
    public class QkExceptionHandler : IQkExceptionHandler
    {
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
