using System;

namespace QkRest
{
    /// <summary>
    /// API response without response data.
    /// </summary>
    public class QkResponse
    {
        /// <summary>
        /// API response error model.
        /// </summary>
        public class QkResponseError
        {
            /// <summary>
            /// Exception type name.
            /// </summary>
            public string Type { get; }

            /// <summary>
            /// Exception message.
            /// </summary>
            public string Message { get; }

            /// <summary>
            /// Exception stack trace.
            /// </summary>
            public string Trace { get; }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="exception">Exception that occured during request processing.</param>
            public QkResponseError(Exception exception)
            {
                Type = exception.GetType().Name;
                Message = exception.Message;
                Trace = exception.StackTrace;
            }
        }

        /// <summary>
        /// API error details.
        /// </summary>
        public QkResponseError Error { get; }

        /// <summary>
        /// Indicates if request excecution was successful or not.
        /// </summary>
        public bool Success { get; } = true;

        /// <summary>
        /// Default constructor for empty and successful API response.
        /// </summary>
        public QkResponse()
        {
        }

        /// <summary>
        /// Constructor for error API response.
        /// </summary>
        /// <param name="exception">Exception that occured during request processing.</param>
        public QkResponse(Exception exception)
        {
            Error = new QkResponseError(exception);
            Success = false;
        }
    }

    /// <summary>
    /// API response with response data.
    /// </summary>
    /// <typeparam name="TData">Response data type.</typeparam>
    public class QkResponse<TData> : QkResponse
    {
        /// <summary>
        /// Response data.
        /// </summary>
        public TData Data { get; }

        /// <summary>
        /// Constructor. Accepts response data model.
        /// </summary>
        /// <param name="data">Response data.</param>
        public QkResponse(TData data)
        {
            Data = data;
        }

        /// <summary>
        /// Constructor. Accepts API exception in case when request failed to process.
        /// </summary>
        /// <param name="exception">Exception that occured during request processing.</param>
        public QkResponse(Exception exception) : base(exception)
        {
        }
    }
}
