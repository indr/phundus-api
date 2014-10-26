namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System.Runtime.Serialization;
    using Articles;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class ReservationAmountChanged : DomainEvent
    {
        public ReservationAmountChanged(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId,
            int amount)
        {
            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            ReservationId = reservationId.Id;
            Amount = amount;
        }

        protected ReservationAmountChanged()
        {
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; protected set; }

        [DataMember(Order = 3)]
        public string ReservationId { get; protected set; }

        [DataMember(Order = 4)]
        public int Amount { get; protected set; }

        [DataMember(Order = 5)]
        public int Delta { get; protected set; }
    }
}