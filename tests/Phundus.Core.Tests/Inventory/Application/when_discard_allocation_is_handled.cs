namespace Phundus.Core.Tests.Inventory.Application
{
    using Core.Inventory.Application.Commands;
    using Machine.Fakes;
    using Machine.Specifications;

    [Subject(typeof (DiscardAllocationHandler))]
    public class when_discard_allocation_is_handled :
        stock_allocation_concern<DiscardAllocation, DiscardAllocationHandler>
    {
        private Establish ctx = () => command = new DiscardAllocation(_organizationId, _articleId, _stockId, _allocationId);

        public It should_call_discard_allocation = () => _stock.WasToldTo(x => x.DiscardAllocation(_allocationId));

        public It should_save_to_repository = () => _repository.WasToldTo(x => x.Save(_stock));
    }
}