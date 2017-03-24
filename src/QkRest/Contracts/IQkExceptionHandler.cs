using System;
using System.Net;

namespace QkRest.Contracts
{
    public interface IQkExceptionHandler
    {
        object HandleException(Exception exception, out HttpStatusCode code);
    }
}
