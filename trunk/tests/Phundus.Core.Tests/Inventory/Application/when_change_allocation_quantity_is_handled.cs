namespace Phundus.Core.Tests.Inventory.Application
{
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Management;
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

    [Subject(typeof(ChangeAllocationQuantityHandler))]
    public class when_change_allocation_quantity_with_default_stock_id_is_handled :
        stock_allocation_concern<ChangeAllocationQuantity, ChangeAllocationQuantityHandler>
    {
        private static int _newQuantity = 2;

        private Establish ctx =
            () => command = new ChangeAllocationQuantity(_organizationId, _articleId, StockId.Default, _allocationId, _newQuantity);

        public It should_call_change_allocation_quantity =
            () => _stock.WasToldTo(x => x.ChangeAllocationQuantity(_allocationId, _newQuantity));

        public It should_save_to_repository = () => _repository.WasToldTo(x => x.Save(_stock));
    }
}