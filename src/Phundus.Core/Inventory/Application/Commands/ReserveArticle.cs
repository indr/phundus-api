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
            ReservationId reservationId, Period period, int quantity)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");
            AssertionConcern.AssertArgumentGreaterThan(quantity, 0, "Quantity must be greater than zero.");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            ArticleId = articleId;
            OrderId = orderId;
            ReservationId = reservationId;
            Period = period;
            Quantity = quantity;
        }

        public UserId InitiatorId { get; private set; }

        public OrganizationId OrganizationId { get; private set; }

        public ArticleId ArticleId { get; private set; }

        public OrderId OrderId { get; private set; }

        public ReservationId ReservationId { get; private set; }

        public Period Period { get; private set; }

        public int Quantity { get; private set; }
    }

    public class ReserveArticleHandler : IHandleCommand<ReserveArticle>
    {
        public IReservationRepository Repository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(ReserveArticle command)
        {
            MemberInRole.ActiveMember(command.OrganizationId, command.InitiatorId);

            var reservation = new Reservation(command.ReservationId, command.OrganizationId, command.ArticleId, command.OrderId,
                command.Period, command.Quantity);

            Repository.Save(reservation);
        }
    }
}