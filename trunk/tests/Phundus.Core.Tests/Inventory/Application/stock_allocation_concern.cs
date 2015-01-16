namespace Phundus.Core.Tests.Inventory.Application
{
    using System;
    using Common.Domain.Model;
    using Core.Cqrs;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using Core.Inventory.Domain.Model.Reservations;
    using Machine.Fakes;
    using Machine.Specifications;

    public class stock_allocation_concern<TCommand, THandler> : stock_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static AllocationId _allocationId = new AllocationId();
        protected static ReservationId _reservationId = new ReservationId();
        protected static Period _period = new Period(DateTime.Today, DateTime.Today.AddDays(1));
        protected static int _quantity = 1;
        private static IArticleRepository _articleRepository;
        private static Article _article;

        private Establish ctx = () =>
        {
            _article = new Article(_articleId, _organizationId.Id, "Article");
            _article.CreateStock(_stockId);
            _articleRepository = depends.on<IArticleRepository>();
            _articleRepository.WhenToldTo(x => x.GetById(_organizationId.Id, _articleId.Id)).Return(_article);
        };
    }
}