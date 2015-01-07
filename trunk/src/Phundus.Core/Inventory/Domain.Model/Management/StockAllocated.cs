namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Reservations;

    public class StockAllocated : DomainEvent
    {
        public StockAllocated(OrganizationId organizationId, StockId stockId, AllocationId allocationId, ReservationId reservationId, Period period, int quantity)
        {
            OrganizationId = organizationId.Id;
            StockId = stockId.Id;
            AllocationId = allocationId.Id;
            ReservationId = reservationId.Id;
            FromUtc = period.FromUtc;
            ToUtc = period.ToUtc;
            Quantity = quantity;
        }

        public int OrganizationId { get; protected set; }
        public string StockId { get; protected set; }
        public Guid AllocationId { get; protected set; }
        public string ReservationId { get; protected set; }
        public DateTime FromUtc { get; protected set; }
        public DateTime ToUtc { get; protected set; }
        public int Quantity { get; protected set; }
    }
}