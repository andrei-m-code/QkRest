namespace QkRest.Exceptions
{
    /// <summary>
    /// Not found exception.
    /// </summary>
    public class QkNotFoundException : QkException
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public QkNotFoundException()
        {
        }

        /// <summary>
        /// Constructor that takes error message.
        /// </summary>
        public QkNotFoundException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
