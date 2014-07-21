namespace Phundus.Rest.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Web;

    [Serializable]
    public class HttpForbiddenException : HttpException
    {
        public HttpForbiddenException() : base(403, "")
        {
        }

        public HttpForbiddenException(string message) : base(403, message)
        {
        }

        public HttpForbiddenException(string message, Exception innerException) : base(403, message, innerException)
        {
        }

        protected HttpForbiddenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}