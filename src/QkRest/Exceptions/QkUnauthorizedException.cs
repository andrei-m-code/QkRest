namespace QkRest.Exceptions
{
    /// <summary>
    /// Unauthorized exception.
    /// </summary>
    public class QkUnauthorizedException : QkException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public QkUnauthorizedException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public QkUnauthorizedException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
