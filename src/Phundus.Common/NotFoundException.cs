namespace Phundus.Common
{
    using System;
    using System.Runtime.Serialization;
    using Domain.Model;

    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string format, object arg0) : base(String.Format(format, arg0))
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class AggregateNotFoundException : NotFoundException
    {
        public AggregateNotFoundException(string name, GuidIdentity id)
            : base(String.Format("Aggregate {0} {1} not found.", name, id))
        {
        }
    }
}