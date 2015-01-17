namespace Phundus.Core.Tests.Inventory.Domain.Model.Management.Stock
{
    using System;
    using Common.Domain.Model;
    using Core.Inventory.Domain.Model.Management;
    using Core.Inventory.Domain.Model.Reservations;

    public class stock_allocation_concern : stock_concern
    {
        protected static AllocationId _allocationId = new AllocationId();
        protected static ReservationId _reservationId = new ReservationId();
        protected static Period _period = new Period(DateTime.Today, DateTime.Today.AddDays(1));
        protected static int _quantity = 1;
    }
}