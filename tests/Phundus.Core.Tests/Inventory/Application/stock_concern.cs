namespace Phundus.Core.Tests.Inventory.Application
{
    using Common.Cqrs;
    using Core.Cqrs;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class stock_concern<TCommand, THandler> : handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand> where TCommand : ICommand
    {
        protected static UserId _initiatorId = new UserId(1);
        protected static IMemberInRole _memberInRole;
        protected static IStockRepository _repository;
        protected static StockId _stockId = new StockId("Stock-1");
        protected static OrganizationId _organizationId = new OrganizationId(2);
        protected static ArticleId _articleId = new ArticleId(3);
        protected static Stock _stock;       

        private Establish ctx = () =>
        {
            _memberInRole = depends.on<IMemberInRole>();
            _stock = MockRepository.GenerateStub<Stock>(_organizationId, _articleId, _stockId);
            _repository = depends.on<IStockRepository>();
            _repository.Expect(x => x.Get(_organizationId, _stockId)).Return(_stock);
        };
    }
}