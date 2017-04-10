namespace QkRest.Exceptions
{
    /// <summary>
    /// Unauthorized exception.
    /// </summary>
    public class QkUnauthorizedException : QkException
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public QkUnauthorizedException()
        {
        }

        /// <summary>
        /// Constructor that takes error message.
        /// </summary>
        public QkUnauthorizedException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
