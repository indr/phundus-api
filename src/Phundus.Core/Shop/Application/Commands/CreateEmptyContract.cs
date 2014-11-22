namespace Phundus.Core.Shop.Application.Commands
{
    using Cqrs;
    using Ddd;
    using Domain.Model.Identity;
    using Domain.Model.Renting;
    using IdentityAndAccess.Queries;

    public class CreateEmptyContract
    {
        public int OrganizationId { get; set; }
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