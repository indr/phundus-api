namespace Phundus.Common.Domain.Model
{
    using System;

    public class CorrelationId : Identity<string>
    {
        public CorrelationId() : this(Guid.NewGuid())
        {
        }

        public CorrelationId(Guid id) : this(id.ToString())
        {
        }

        public CorrelationId(string id) : base(id)
        {
        }
    }
}