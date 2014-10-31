namespace Phundus.Core.Inventory.Application.Data
{
    using System;

    public class ReservationData
    {
        public virtual int ConcurrencyVersion { get; set; }
        public virtual int ArticleId { get; set; }
        public virtual string ReservationId { get; set; }
        public virtual int OrganizationId { get; set; }
        public virtual DateTime CreatedUtc { get; set; }
        public virtual DateTime UpdatedUtc { get; set; }

        public virtual int Amount { get; set; }
        public virtual DateTime FromUtc { get; set; }
        public virtual DateTime ToUtc { get; set; }
    }
}