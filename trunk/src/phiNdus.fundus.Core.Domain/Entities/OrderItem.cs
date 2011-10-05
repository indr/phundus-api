using System;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public virtual Order Order { get; set; }

        public virtual int Amount { get; set; }

        public virtual DateTime From { get; set; }

        public virtual DateTime To { get; set; }

        public virtual Article Article { get; set; }
    }
}