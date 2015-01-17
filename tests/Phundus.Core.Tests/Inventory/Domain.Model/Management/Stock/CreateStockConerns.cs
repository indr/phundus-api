namespace Phundus.Core.Tests.Inventory.Domain.Model.Management.Stock
{
    using Core.Inventory.Domain.Model.Management;
    using Machine.Specifications;

    [Subject(typeof (Stock))]
    public class when_discard_allocation : stock_allocation_concern
    {
        private Establish ctx = () =>
        {
            _sut.Allocate(_allocationId, _reservationId, _period, _quantity);
            _sut.MutatingEvents.Clear();
        };

        private Because of = () => _sut.DiscardAllocation(_allocationId);

        public It should_have_mutating_event_allocation_discarded = () =>
        {
            var e = AssertMutatingEvent<AllocationDiscarded>(0);
            e.OrganizationId.ShouldEqual(_organizationId.Id);
            e.ArticleId.ShouldEqual(_articleId.Id);
            e.StockId.ShouldEqual(_stockId.Id);
            e.AllocationId.ShouldEqual(_allocationId.Id);
        };
    }

    [Subject(typeof(Stock))]
    public class when_a_stock_is_created : stock_concern
    {
        public Because of = () =>
            _sut = new Stock(_organizationId, _articleId, _stockId);

        public It should_have_mutating_event_stock_created = () =>
        {
            var evnt = AssertMutatingEvent<StockCreated>(0);
            evnt.ArticleId.ShouldEqual(_articleId.Id);
            evnt.OrganizationId.ShouldEqual(_organizationId.Id);
            evnt.StockId.ShouldEqual(_stockId.Id);
        };

        public It should_have_article_id = () => _sut.ArticleId.ShouldEqual(_articleId);

        public It should_have_organization_id = () => _sut.OrganizationId.ShouldEqual(_organizationId);

        public It should_have_stock_id = () => _sut.StockId.ShouldEqual(_stockId);

        public It should_have_no_allocations = () => _sut.Allocations.ShouldBeEmpty();
    }
}