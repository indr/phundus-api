namespace Phundus.Core.Shop.Contracts.Commands
{
    using System;
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;

    public class CreateNewEmptyContract
    {
        public int OrganizationId { get; set; }
        public int InitiatorId { get; set; }
        public int ContractId { get; set; }
    }

    public class CreateNewEmptyContractHandler : IHandleCommand<CreateNewEmptyContract>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IContractRepository Repository { get; set; }

        public void Handle(CreateNewEmptyContract command)
        {
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            var contract = new Contract();
            
            command.ContractId = Repository.Add(contract);

            EventPublisher.Publish(new ContractCreated());
        }
    }
}