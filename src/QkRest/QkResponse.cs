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
            public string Type { get; private set; }

            /// <summary>
            /// Exception message.
            /// </summary>
            public string Message { get; private set; }

            /// <summary>
            /// Exception stack trace.
            /// </summary>
            public string Trace { get; private set; }

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
        /// API populates this property in case when exception occurs during request processing.
        /// </summary>
        public QkResponseError Error { get; private set; }

        /// <summary>
        /// Was request successful.
        /// </summary>
        public bool IsSuccessful { get; private set; } = true;

        /// <summary>
        /// Constructor. Default constructor for empty and successful API response.
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
            IsSuccessful = false;
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
        public TData Data { get; private set; }

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
