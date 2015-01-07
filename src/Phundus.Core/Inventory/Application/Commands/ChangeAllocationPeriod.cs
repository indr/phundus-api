namespace Phundus.Core.Inventory.Application.Commands
{
    using Common;
    using Common.Cqrs;
    using Common.Domain.Model;
    using Cqrs;
    using Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class ChangeAllocationPeriod : ICommand
    {
        public ChangeAllocationPeriod(OrganizationId organizationId, StockId stockId, AllocationId allocationId, Period period)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");
            AssertionConcern.AssertArgumentNotNull(allocationId, "Allocation id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");

            OrganizationId = organizationId;
            StockId = stockId;
            AllocationId = allocationId;
            Period = period;
        }

        public OrganizationId OrganizationId { get; private set; }
        public StockId StockId { get; private set; }
        public AllocationId AllocationId { get; private set; }
        public Period Period { get; private set; }
    }

    public class ChangeAllocationPeriodHandler : IHandleCommand<ChangeAllocationPeriod>
    {
        public IStockRepository Repository { get; set; }

        public void Handle(ChangeAllocationPeriod command)
        {
            var stock = Repository.Get(command.OrganizationId, command.StockId);

            stock.ChangeAllocationPeriod(command.AllocationId, command.Period);

            Repository.Save(stock);
        }
    }
}