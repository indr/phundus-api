namespace Phundus.Core.Tests.Shop
{
    using System;
    using Core.Shop.Contracts.Commands;
    using Core.Shop.Contracts.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (CreateEmptyContractHandler))]
    public class when_create_empty_contract_is_handled :
        contract_handler_concern<CreateEmptyContract, CreateEmptyContractHandler>
    {
        public const int organizationId = 1;
        public const int initiatorId = 2;
        public const int contractId = 3;
        public const int userId = 4;

        public Establish c = () =>
        {
            repository.setup(x => x.Add(Arg<Contract>.Is.NotNull)).Return(contractId);
            borrowerService.setup(x => x.ById(userId)).Return(new Borrower(userId, "First", "Last", "mail@domain.tld"));
            command = new CreateEmptyContract
            {
                OrganizationId = organizationId,
                InitiatorId = initiatorId,
                UserId = userId
            };
        };

        public It should_add_to_repository = () => repository.WasToldTo(x => x.Add(Arg<Contract>.Is.NotNull));

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId, initiatorId));

        public It should_publish_contract_created = () => publisher.WasToldTo(x => x.Publish(
            Arg<ContractCreated>.Matches(p => p.BorrowerEmail == "mail@domain.tld"
                                              && p.BorrowerFirstName == "First"
                                              && p.BorrowerId == userId
                                              && p.BorrowerLastName == "Last"
                                              && p.ContractId == contractId
                                              && p.CreatedOn > DateTime.UtcNow.AddSeconds(-1)
                                              && p.OrganizationId == organizationId)));

        public It should_set_contract_id = () => command.ContractId.ShouldEqual(contractId);
    }
}