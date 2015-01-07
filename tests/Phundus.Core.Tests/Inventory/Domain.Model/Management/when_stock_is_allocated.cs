namespace Phundus.Core.Tests.Inventory.Domain.Model.Management
{
    using Core.Inventory.Domain.Model.Management;
    using Machine.Specifications;

    public class when_stock_is_allocated : stock_allocation_concern
    {
        public Because of = () => _sut.Allocate(_allocationId, _reservationId, _period, _quantity);

        public It should_have_mutating_event_stock_allocated = () =>
        {
            var e = AssertMutatingEvent<StockAllocated>(0);
            e.OrganizationId.ShouldEqual(_organizationId.Id);
            e.StockId.ShouldEqual(_stockId.Id);
            e.AllocationId.ShouldEqual(_allocationId.Id);
            e.ReservationId.ShouldEqual(_reservationId.Id);
            e.FromUtc.ShouldEqual(_period.FromUtc);
            e.ToUtc.ShouldEqual(_period.ToUtc);
            e.Quantity.ShouldEqual(_quantity);
        };

        public It should_have_allocatoin = () => _sut.Allocations.ShouldContain(c => Equals(c.AllocationId, _allocationId));
    }
}