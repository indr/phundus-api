namespace Phundus.Shop.Contracts.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class ContractCreated : DomainEvent
    {
        [DataMember(Order = 1)]
        public int ContractId { get; set; }

        [DataMember(Order = 2)]
        public Guid OrganizationId { get; set; }

        [DataMember(Order = 3)]
        public Guid BorrowerId { get; set; }

        [DataMember(Order = 4)]
        public string BorrowerFirstName { get; set; }

        [DataMember(Order = 5)]
        public string BorrowerLastName { get; set; }

        [DataMember(Order = 6)]
        public string BorrowerEmail { get; set; }
    }
}