namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class ReservationCreated : DomainEvent
    {
        protected ReservationCreated()
        {
            
        }

        [DataMember(Order = 1)]
        public string OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public string ArticleId { get; protected set; }

        [DataMember(Order = 3)]
        public string ReservationId { get; protected set; }

        [DataMember(Order = 4)]
        public DateTime FromUtc { get; protected set; }

        [DataMember(Order = 5)]
        public DateTime ToUtc { get; protected set; }

        [DataMember(Order = 6)]
        public int Amount { get; protected set; }
    }
}