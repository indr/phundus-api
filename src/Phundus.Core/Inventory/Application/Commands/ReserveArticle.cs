namespace Phundus.Core.Inventory.Application.Commands
{
    using System;
    using Common;
    using Common.Cqrs;
    using Common.Domain.Model;
    using Cqrs;
    using Domain.Model.Catalog;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;
    using Shop.Domain.Model.Ordering;

    public class ReserveArticle : ICommand
    {
        public ReserveArticle(UserId initiatorId, OrganizationId organizationId, ArticleId articleId, OrderId orderId,
            CorrelationId correlationId, DateTime fromUtc, DateTime toUtc, int quantity)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(fromUtc, "From UTC must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(toUtc, "To UTC must be provided.");
            AssertionConcern.AssertArgumentGreaterThan(quantity, 0, "Quantity must be greater than zero.");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            ArticleId = articleId;
            OrderId = orderId;
            CorrelationId = correlationId;
            FromUtc = fromUtc;
            ToUtc = toUtc;
            Quantity = quantity;
        }

        public UserId InitiatorId { get; private set; }

        public OrganizationId OrganizationId { get; private set; }

        public ArticleId ArticleId { get; private set; }

        public OrderId OrderId { get; private set; }

        public CorrelationId CorrelationId { get; private set; }

        public DateTime FromUtc { get; private set; }

        public DateTime ToUtc { get; private set; }

        public int Quantity { get; private set; }

        public string ResultingReservationId { get; set; }
    }

    public class ReserveArticleHandler : IHandleCommand<ReserveArticle>
    {
        public IReservationRepository Repository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(ReserveArticle command)
        {
            MemberInRole.ActiveMember(command.OrganizationId, command.InitiatorId);

            var timeRange = new Period(command.FromUtc, command.ToUtc);
            var reservation = new Reservation(command.OrganizationId, command.ArticleId, Repository.GetNextIdentity(), command.OrderId,
                command.CorrelationId, timeRange, command.Quantity);

            Repository.Save(reservation);

            command.ResultingReservationId = reservation.ReservationId.Id;
        }
    }
}