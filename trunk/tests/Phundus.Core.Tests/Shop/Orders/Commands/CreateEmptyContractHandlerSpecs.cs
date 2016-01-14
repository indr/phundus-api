namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using System;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Contracts.Commands;
    using Phundus.Shop.Contracts.Model;
    using Phundus.Tests.Shop;
    using Rhino.Mocks;

    [Subject(typeof (CreateEmptyContractHandler))]
    public class when_create_empty_contract_is_handled :
        contract_handler_concern<CreateEmptyContract, CreateEmptyContractHandler>
    {
        public static Guid organizationId;
        public const int initiatorId = 2;
        public const int contractId = 3;
        public const int userId = 4;

        public Establish c = () =>
        {
            organizationId = Guid.NewGuid();
            repository.setup(x => x.Add(Arg<Contract>.Is.NotNull)).Return(contractId);
            lesseeService.setup(x => x.GetById(userId)).Return(BorrowerFactory.Create(userId, "First", "Last", "Street", "Postcode", "City","EmailAddress"));
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
            Arg<ContractCreated>.Matches(p => p.BorrowerEmail == "EmailAddress"
                                              && p.BorrowerFirstName == "First"
                                              && p.BorrowerId == userId
                                              && p.BorrowerLastName == "Last"
                                              && p.ContractId == contractId
                                              && p.OccuredOnUtc > DateTime.UtcNow.AddSeconds(-1)
                                              && p.OrganizationId == organizationId)));

        public It should_set_contract_id = () => command.ContractId.ShouldEqual(contractId);
    }
}