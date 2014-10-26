namespace Phundus.Core.Inventory.Application
{
    using System;
    using System.Globalization;
    using Common.Domain.Model;
    using Cqrs;
    using Domain.Model.Articles;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;

    public class CreateReservation
    {
        public CreateReservation(int initiatorId, string organizationId, string articleId, DateTime fromUtc, DateTime toUtc, int amount)
        {
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            ArticleId = articleId;
            FromUtc = fromUtc;
            ToUtc = toUtc;
            Amount = amount;
        }

        public int InitiatorId { get; private set; }

        public string OrganizationId { get; private set; }

        public string ArticleId { get; private set; }

        public DateTime FromUtc { get; private set; }

        public DateTime ToUtc { get; private set; }

        public int Amount { get; private set; }

        public string ResultingReservationId { get; set; }
    }

    public class CreateReservationHandler : IHandleCommand<CreateReservation>
    {
        public IReservationRepository Repository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(CreateReservation command)
        {
            var initiatorId = new UserId(command.InitiatorId.ToString(CultureInfo.InvariantCulture));
            var organizationId = new OrganizationId(command.OrganizationId);
            MemberInRole.ActiveMember(organizationId, initiatorId);

            var articleId = new ArticleId(command.ArticleId);
            var timeRange = new TimeRange(command.FromUtc, command.ToUtc);
            var reservation = new Reservation(organizationId, articleId, Repository.GetNextIdentity(), timeRange,
                command.Amount);

            Repository.Save(reservation);

            command.ResultingReservationId = reservation.ReservationId.Id;
        }
    }
}