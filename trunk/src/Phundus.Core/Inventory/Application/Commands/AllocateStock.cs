namespace Phundus.Core.Inventory.Application.Commands
{
    using System;
    using Common;
    using Common.Cqrs;
    using Common.Domain.Model;
    using Cqrs;
    using Domain.Model.Catalog;
    using Domain.Model.Management;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class AllocateStock : ICommand
    {
        public AllocateStock(OrganizationId organizationId, ArticleId articleId, StockId stockId, AllocationId allocationId, ReservationId reservationId, Period period, int quantity)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");
            AssertionConcern.AssertArgumentNotNull(allocationId, "Allocation id must be provided.");
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");
            AssertionConcern.AssertArgumentGreaterThanZero(quantity, "Quantity must be greater than zero.");

            OrganizationId = organizationId;
            ArticleId = articleId;
            StockId = stockId;
            AllocationId = allocationId;
            ReservationId = reservationId;
            Period = period;
            Quantity = quantity;
        }

        public OrganizationId OrganizationId { get; private set; }
        public ArticleId ArticleId { get; private set; }
        public StockId StockId { get; private set; }
        public AllocationId AllocationId { get; private set; }
        public ReservationId ReservationId { get; private set; }
        public Period Period { get; private set; }
        public int Quantity { get; private set; }
    }

    public class AllocateStockHandler : AllocationHandlerBase, IHandleCommand<AllocateStock>
    {
        public AllocateStockHandler(IStockRepository stockRepository, IArticleRepository articleRepository) : base(stockRepository, articleRepository)
        {
        }

        public void Handle(AllocateStock command)
        {
            var stock = GetStock(command.OrganizationId, command.ArticleId, command.StockId);

            stock.Allocate(command.AllocationId, command.ReservationId, command.Period, command.Quantity);

            StockRepository.Save(stock);
        }
    }
}