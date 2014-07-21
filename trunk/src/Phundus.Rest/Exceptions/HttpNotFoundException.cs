namespace Phundus.Rest.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Web;

    [Serializable]
    public class HttpNotFoundException : HttpException
    {
        public HttpNotFoundException()
            : base(404, "")
        {
        }

        public HttpNotFoundException(string message)
            : base(404, message)
        {
        }

        public HttpNotFoundException(string message, Exception innerException)
            : base(404, message, innerException)
        {
        }

        protected HttpNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}