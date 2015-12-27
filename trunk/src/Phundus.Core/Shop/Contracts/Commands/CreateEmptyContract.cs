namespace Phundus.Core.Shop.Contracts.Commands
{
    using System;
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;
    using Services;

    public class CreateEmptyContract
    {
        public Guid OrganizationId { get; set; }
        public int InitiatorId { get; set; }
        public int ContractId { get; set; }
        public int UserId { get; set; }
    }

    public class CreateEmptyContractHandler : IHandleCommand<CreateEmptyContract>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IContractRepository Repository { get; set; }

        public IBorrowerService Borrower { get; set; }

        public void Handle(CreateEmptyContract command)
        {
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            var contract = new Contract(
                command.OrganizationId,
                Borrower.ById(command.UserId));

            var contractId = Repository.Add(contract);
            
            command.ContractId = contractId;
            
            EventPublisher.Publish(new ContractCreated
            {
                BorrowerEmail = contract.Borrower.EmailAddress,
                BorrowerFirstName = contract.Borrower.FirstName,
                BorrowerId = contract.Borrower.Id,
                BorrowerLastName = contract.Borrower.LastName,
                ContractId = contractId,
                OrganizationId = contract.OrganizationId,
            });
        }
    }
}