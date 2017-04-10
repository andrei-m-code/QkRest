using System;

namespace QkRest.Exceptions
{
    /// <summary>
    /// Base Qk exception.
    /// </summary>
    public class QkException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public QkException()
        {
        }

        /// <summary>
        /// Constructor that takes error message.
        /// </summary>
        public QkException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
