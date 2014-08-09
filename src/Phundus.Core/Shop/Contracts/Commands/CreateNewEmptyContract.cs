namespace Phundus.Core.Shop.Contracts.Commands
{
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;
    using Services;

    public class CreateNewEmptyContract
    {
        public int OrganizationId { get; set; }
        public int InitiatorId { get; set; }
        public int ContractId { get; set; }
        public int UserId { get; set; }
    }

    public class CreateNewEmptyContractHandler : IHandleCommand<CreateNewEmptyContract>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IContractRepository Repository { get; set; }

        public IBorrowerService Borrower { get; set; }

        public void Handle(CreateNewEmptyContract command)
        {
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            var contract = new Contract(
                Borrower.ById(command.UserId));

            command.ContractId = Repository.Add(contract);

            EventPublisher.Publish(new ContractCreated());
        }
    }
}