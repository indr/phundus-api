namespace Phundus.Core.Tests.Inventory.Domain.Model.Management.Stock
{
    using System;
    using Core.Inventory.Domain.Model.Management;
    using Machine.Specifications;

    [Subject(typeof (Stock))]
    public class when_allocation_quantity_of_none_present_allocation_is_changed : stock_allocation_concern
    {
        private static Exception _exception;

        private Because of =
            () => _exception = Catch.Exception(() => _sut.ChangeAllocationQuantity(_allocationId, 1));

        public It should_have_exception_message =
            () =>
                _exception.Message.ShouldEqual(String.Format("Allocation {0} not found.", _allocationId.Id));

        public It should_throw_allocation_not_found_exception =
            () => _exception.ShouldBeOfExactType<AllocationNotFoundException>();
    }

    [Subject(typeof (Stock))]
    public class when_allocation_quantity_is_changed : stock_allocation_concern
    {
        private static int _newQuantity = _quantity + 1;

        private Establish ctx = () =>
        {
            _sut.Allocate(_allocationId, _reservationId, _period, _quantity);
            _sut.MutatingEvents.Clear();
        };

        public Because of = () => _sut.ChangeAllocationQuantity(_allocationId, _newQuantity);

        public It should_have_mutating_event_allocation_quantity_changed = () =>
        {
            var e = AssertMutatingEvent<AllocationQuantityChanged>(0);
            e.OrganizationId.ShouldEqual(_organizationId.Id);
            e.ArticleId.ShouldEqual(_articleId.Id);
            e.StockId.ShouldEqual(_stockId.Id);
            e.AllocationId.ShouldEqual(_allocationId.Id);
            e.OldQuantity.ShouldEqual(_quantity);
            e.NewQuantity.ShouldEqual(_newQuantity);
        };
    }
}