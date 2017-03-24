using System;

namespace QkRest.Exceptions
{
    /// <summary>
    /// Base tracker exception.
    /// </summary>
    public class QkException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public QkException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public QkException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
