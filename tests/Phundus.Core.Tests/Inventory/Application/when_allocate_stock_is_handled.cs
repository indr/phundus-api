namespace Phundus.Core.Tests.Inventory.Application
{
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Management;
    using Machine.Fakes;
    using Machine.Specifications;

    [Subject(typeof (AllocateStockHandler))]
    public class when_allocate_stock_is_handled :
        stock_allocation_concern<AllocateStock, AllocateStockHandler>
    {
        private Establish ctx = () =>
        {
            command = new AllocateStock(_organizationId, _articleId, _stockId, _allocationId, _reservationId,
                _period, _quantity);
        };

        public It should_call_allocate =
            () => _stock.WasToldTo(x => x.Allocate(_allocationId, _reservationId, _period, _quantity));

        public It should_save_to_repository = () => _repository.WasToldTo(x => x.Save(_stock));
    }

    [Subject(typeof (AllocateStockHandler))]
    public class when_allocate_stock_with_default_stock_id_is_handled :
        stock_allocation_concern<AllocateStock, AllocateStockHandler>
    {
        private Establish ctx = () =>
        {
            command = new AllocateStock(_organizationId, _articleId, StockId.Default, _allocationId, _reservationId,
                _period, _quantity);
        };

        public It should_call_allocate =
            () => _stock.WasToldTo(x => x.Allocate(_allocationId, _reservationId, _period, _quantity));

        public It should_save_to_repository = () => _repository.WasToldTo(x => x.Save(_stock));
    }
}