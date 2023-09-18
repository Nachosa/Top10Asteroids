namespace NASA.Helper.Exceptions
{
    public class NotSuccessfulAPICallException : ApplicationException
    {
        public NotSuccessfulAPICallException(string message) : base(message) { }

    }
}
