namespace QkRest.Exceptions
{
    /// <summary>
    /// Not found exception.
    /// </summary>
    public class QkNotFoundException : QkException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public QkNotFoundException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public QkNotFoundException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
