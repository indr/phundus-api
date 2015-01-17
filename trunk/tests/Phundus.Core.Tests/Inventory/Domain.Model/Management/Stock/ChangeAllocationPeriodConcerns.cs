namespace Phundus.Core.Tests.Inventory.Domain.Model.Management.Stock
{
    using System;
    using Common.Domain.Model;
    using Core.Inventory.Domain.Model.Management;
    using Machine.Specifications;

    [Subject(typeof(Stock))]
    public class when_allocation_period_of_none_present_allocation_is_changed : stock_allocation_concern
    {
        private static Exception _exception;

        private Because of =
            () => _exception = Catch.Exception(() => _sut.ChangeAllocationPeriod(_allocationId, _period));

        public It should_have_exception_message =
            () =>
                _exception.Message.ShouldEqual(String.Format("Allocation {0} not found.", _allocationId.Id));

        public It should_throw_allocation_not_found_exception =
            () => _exception.ShouldBeOfExactType<AllocationNotFoundException>();
    }

    [Subject(typeof (Stock))]
    public class when_allocation_period_is_changed : stock_allocation_concern
    {
        private static Period _newPeriod = _period.ShiftDays(1);

        private Establish ctx = () =>
        {
            _sut.Allocate(_allocationId, _reservationId, _period, _quantity);
            _sut.MutatingEvents.Clear();
        };

        public Because of = () => _sut.ChangeAllocationPeriod(_allocationId, _newPeriod);

        public It should_have_mutating_event_allocation_period_changed = () =>
        {
            var e = AssertMutatingEvent<AllocationPeriodChanged>(0);
            e.OrganizationId.ShouldEqual(_organizationId.Id);
            e.ArticleId.ShouldEqual(_articleId.Id);
            e.StockId.ShouldEqual(_stockId.Id);
            e.AllocationId.ShouldEqual(_allocationId.Id);
            e.OldPeriod.ShouldEqual(_period);
            e.NewPeriod.ShouldEqual(_newPeriod);
        };
    }
}