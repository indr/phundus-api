namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using System.Runtime.Serialization;
    using Catalog;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class ReservationPeriodChanged : DomainEvent
    {
        public ReservationPeriodChanged(OrganizationId organizationId, ArticleId articleId,
            ReservationId reservationId, Period oldPeriod, Period newPeriod)
        {
            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            ReservationId = reservationId.Id;
            OldPeriod = oldPeriod;
            NewPeriod = newPeriod;
        }

        protected ReservationPeriodChanged()
        {
        }

        public Period OldPeriod
        {
            get { return new Period(OldFromUtc, OldToUtc); }
            set
            {
                OldFromUtc = value.FromUtc;
                OldToUtc = value.ToUtc;
            }
        }

        public Period NewPeriod
        {
            get { return new Period(NewFromUtc, NewToUtc); }
            set
            {
                NewFromUtc = value.FromUtc;
                NewToUtc = value.ToUtc;
            }
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; protected set; }

        [DataMember(Order = 3)]
        public string ReservationId { get; protected set; }

        [DataMember(Order = 4)]
        public DateTime OldFromUtc { get; protected set; }

        [DataMember(Order = 5)]
        public DateTime OldToUtc { get; protected set; }

        [DataMember(Order = 6)]
        public DateTime NewFromUtc { get; protected set; }

        [DataMember(Order = 7)]
        public DateTime NewToUtc { get; protected set; }
    }
}