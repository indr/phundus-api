namespace Phundus.Core.Inventory.Application.Commands
{
    using System;
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
        public ReserveArticle(int initiatorId, int organizationId, int articleId, OrderId orderId,
            CorrelationId correlationId,
            DateTime fromUtc, DateTime toUtc, int amount)
        {
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            ArticleId = articleId;
            OrderId = orderId;
            CorrelationId = correlationId;
            FromUtc = fromUtc;
            ToUtc = toUtc;
            Amount = amount;
        }

        public int InitiatorId { get; private set; }

        public int OrganizationId { get; private set; }

        public int ArticleId { get; private set; }

        public OrderId OrderId { get; private set; }

        public CorrelationId CorrelationId { get; private set; }

        public DateTime FromUtc { get; private set; }

        public DateTime ToUtc { get; private set; }

        public int Amount { get; private set; }

        public string ResultingReservationId { get; set; }
    }

    public class ReserveArticleHandler : IHandleCommand<ReserveArticle>
    {
        public IReservationRepository Repository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(ReserveArticle command)
        {
            var initiatorId = new UserId(command.InitiatorId);
            var organizationId = new OrganizationId(command.OrganizationId);
            MemberInRole.ActiveMember(organizationId, initiatorId);

            var articleId = new ArticleId(command.ArticleId);
            var timeRange = new TimeRange(command.FromUtc, command.ToUtc);
            var reservation = new Reservation(organizationId, articleId, Repository.GetNextIdentity(), command.OrderId,
                command.CorrelationId, timeRange, command.Amount);

            Repository.Save(reservation);

            command.ResultingReservationId = reservation.ReservationId.Id;
        }
    }
}