namespace Phundus.Common.Domain.Model
{
    using System;

    public abstract class GuidIdentity : Identity<Guid>
    {
        protected GuidIdentity() : base(Guid.NewGuid())
        {
        }

        protected GuidIdentity(Guid id) : base(id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");
        }
    }
}