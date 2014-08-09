namespace Phundus.Core.Shop.Contracts.Model
{
    using System;
    using Ddd;

    public class ContractCreated : DomainEvent
    {
        public int ContractId { get; set; }
        public int Version { get; set; }
        public int OrganizationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int BorrowerId { get; set; }
        public string BorrowerFirstName { get; set; }
        public string BorrowerLastName { get; set; }
        public string BorrowerEmail { get; set; }
    }
}