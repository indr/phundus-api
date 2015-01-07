namespace Phundus.Core.Tests.Inventory.Application
{
    using System;
    using Common.Domain.Model;
    using Core.Cqrs;
    using Core.Inventory.Domain.Model.Management;
    using Core.Inventory.Domain.Model.Reservations;

    public class stock_allocation_concern<TCommand, THandler> : stock_concern<TCommand, THandler> where THandler : class, IHandleCommand<TCommand>
    {
        protected static AllocationId _allocationId = new AllocationId();
        protected static ReservationId _reservationId = new ReservationId();
        protected static Period _period = new Period(DateTime.Today, DateTime.Today.AddDays(1));
        protected static int _quantity = 1;
    }
}