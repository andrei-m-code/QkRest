using System;
using System.Net;
using QkRest.Exceptions;

namespace QkRest.Sample.Handlers
{
    public class MyExceptionHandler : QkExceptionHandler
    {
        public override object HandleException(Exception exception, out HttpStatusCode code)
        {
            if (exception is ArgumentException)
            {
                return base.HandleException(new QkException(), out code);
            }

            return base.HandleException(exception, out code);
        }
    }
}
