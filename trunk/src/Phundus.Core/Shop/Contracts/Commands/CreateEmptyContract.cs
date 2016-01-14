namespace Phundus.Shop.Contracts.Commands
{
    using System;
    using Cqrs;
    using Ddd;
    using IdentityAccess.Queries;
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

        public IContractRepository ContractRepository { get; set; }

        public ILesseeService LesseeService { get; set; }

        public void Handle(CreateEmptyContract command)
        {
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            var contract = new Contract(
                command.OrganizationId,
                LesseeService.GetById(command.UserId));

            var contractId = ContractRepository.Add(contract);
            
            command.ContractId = contractId;
            
            EventPublisher.Publish(new ContractCreated
            {
                BorrowerEmail = contract.Lessee.EmailAddress,
                BorrowerFirstName = contract.Lessee.FirstName,
                BorrowerId = contract.Lessee.Id,
                BorrowerLastName = contract.Lessee.LastName,
                ContractId = contractId,
                OrganizationId = contract.OrganizationId,
            });
        }
    }
}