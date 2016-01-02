namespace Phundus.Common.Domain.Model
{
    using System;

    public class StoreId : Identity<Guid>
    {
        public StoreId() : base(Guid.NewGuid())
        {
        }

        public StoreId(Guid value) : base(value)
        {
        }
    }
}