namespace Phundus.Core.Tests.Inventory.Application
{
    using Core.Inventory.Application.Commands;
    using Machine.Fakes;
    using Machine.Specifications;

    [Subject(typeof (ChangeAllocationQuantityHandler))]
    public class when_change_allocation_quantity_is_handled :
        stock_allocation_concern<ChangeAllocationQuantity, ChangeAllocationQuantityHandler>
    {
        private static int _newQuantity = 2;

        private Establish ctx =
            () => command = new ChangeAllocationQuantity(_organizationId, _articleId, _stockId, _allocationId, _newQuantity);

        public It should_call_change_allocation_quantity =
            () => _stock.WasToldTo(x => x.ChangeAllocationQuantity(_allocationId, _newQuantity));

        public It should_save_to_repository = () => _repository.WasToldTo(x => x.Save(_stock));
    }
}