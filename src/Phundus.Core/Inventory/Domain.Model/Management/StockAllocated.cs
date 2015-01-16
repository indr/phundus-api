namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Runtime.Serialization;
    using Catalog;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Reservations;

    [DataContract]
    public class StockAllocated : DomainEvent
    {
        public StockAllocated(OrganizationId organizationId, StockId stockId, AllocationId allocationId, ReservationId reservationId, Period period, int quantity, AllocationStatus status)
        {
            OrganizationId = organizationId.Id;
            StockId = stockId.Id;
            AllocationId = allocationId.Id;
            ReservationId = reservationId.Id;
            FromUtc = period.FromUtc;
            ToUtc = period.ToUtc;
            Quantity = quantity;
            AllocationStatus = status;
        }

        protected StockAllocated() { }

        [DataMember(Order = 1)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public string StockId { get; protected set; }

        [DataMember(Order = 3)]
        public Guid AllocationId { get; protected set; }

        [DataMember(Order = 4)]
        public string ReservationId { get; protected set; }

        [DataMember(Order = 5)]
        public DateTime FromUtc { get; protected set; }

        [DataMember(Order = 6)]
        public DateTime ToUtc { get; protected set; }

        [DataMember(Order = 7)]
        public int Quantity { get; protected set; }

        [DataMember(Order = 8)]
        public AllocationStatus AllocationStatus { get; protected set; }
    }
}