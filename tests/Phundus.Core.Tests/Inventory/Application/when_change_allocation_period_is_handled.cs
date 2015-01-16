namespace Phundus.Core.Tests.Inventory.Application
{
    using System;
    using Common.Domain.Model;
    using Core.Inventory.Application.Commands;
    using Machine.Fakes;
    using Machine.Specifications;

    [Subject(typeof (ChangeAllocationPeriodHandler))]
    public class when_change_allocation_period_is_handled :
        stock_allocation_concern<ChangeAllocationPeriod, ChangeAllocationPeriodHandler>
    {
        private static Period _newPeriod = new Period(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3));

        private Establish ctx =
            () => command = new ChangeAllocationPeriod(_organizationId, _articleId, _stockId, _allocationId, _newPeriod);

        public It should_call_change_allocation_period =
            () => _stock.WasToldTo(x => x.ChangeAllocationPeriod(_allocationId, _newPeriod));

        public It should_save_to_repository =
            () => _repository.WasToldTo(x => x.Save(_stock));
    }
}