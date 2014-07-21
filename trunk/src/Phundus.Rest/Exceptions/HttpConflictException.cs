namespace Phundus.Rest.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Web;

    [Serializable]
    public class HttpConflictException : HttpException
    {
        public HttpConflictException()
            : base(409, "")
        {
        }

        public HttpConflictException(string message)
            : base(409, message)
        {
        }

        public HttpConflictException(string message, Exception innerException)
            : base(409, message, innerException)
        {
        }

        protected HttpConflictException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}