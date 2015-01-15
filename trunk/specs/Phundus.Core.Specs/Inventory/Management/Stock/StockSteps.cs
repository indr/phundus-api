namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using Contexts;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Rhino.Mocks;
    using TechTalk.SpecFlow;

    [Binding]
    public class StockSteps
    {
        private readonly Container _container;
        private readonly StockContext _context;
        private readonly IStockRepository _stockRepository;

        public StockSteps(Container container, StockContext context)
        {
            _container = container;
            _context = context;
            _stockRepository = _container.Depend.On<IStockRepository>();
        }

        [Given(@"stock created (.*), article (.*), organization (.*)")]
        public void StockCreated(string stockId, int articleId, int organizationId)
        {
            _context.OrganizationId = new OrganizationId(organizationId);
            _context.ArticleId = new ArticleId(articleId);
            _context.StockId = new StockId(stockId);
            _context.PastEvents.Add(new StockCreated(_context.OrganizationId, _context.ArticleId, _context.StockId));

            _stockRepository.Expect(
                x => x.Get(Arg<OrganizationId>.Is.Anything, Arg<StockId>.Is.Anything))
                .WhenCalled(a => a.ReturnValue = _context.Sut).Return(null);

            _stockRepository.Expect(x => x.Save(Arg<Stock>.Is.NotNull))
                .WhenCalled(a => _context.MutatingEvents.Events = ((Stock)a.Arguments[0]).MutatingEvents);
        }

        [When(@"Increase quantity in inventory of (.*) as of (.*)")]
        public void WhenIncreaseQuantityInInventory(int quantity, DateTime asOfUtc)
        {
            _container.Resolve<ChangeQuantityInInventoryHandler>()
                .Handle(new ChangeQuantityInInventory(_context.InitiatorId, _context.OrganizationId, _context.ArticleId,
                    _context.StockId, quantity, asOfUtc, null));
        }

        [When(@"Decrease quantity in inventory of (.*) as of (.*)")]
        public void WhenDecreaseQuantityInInventory(int quantity, DateTime asOfUtc)
        {
            _container.Resolve<ChangeQuantityInInventoryHandler>()
                .Handle(new ChangeQuantityInInventory(_context.InitiatorId, _context.OrganizationId, _context.ArticleId,
                    _context.StockId, quantity * -1, asOfUtc, null));
        }
    }
}