namespace Phundus.Core.Tests.Shop
{
    using Core.Shop.Contracts.Commands;
    using Core.Shop.Contracts.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (CreateNewEmptyContractHandler))]
    public class when_create_new_empty_contract_is_handled :
        contract_handler_concern<CreateNewEmptyContract, CreateNewEmptyContractHandler>
    {
        public const int organizationId = 1;
        public const int initiatorId = 2;
        public const int contractId = 3;

        public Establish c = () =>
        {
            repository.setup(x => x.Add(Arg<Contract>.Is.Anything)).Return(contractId);

            command = new CreateNewEmptyContract
            {
                OrganizationId = organizationId,
                InitiatorId = initiatorId
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId, initiatorId));

        public It should_add_to_repository = () => repository.WasToldTo(x => x.Add(Arg<Contract>.Is.NotNull));

        public It should_publish_contract_created = () => publisher.WasToldTo(x => x.Publish(Arg<ContractCreated>.Is.NotNull));

        public It should_set_contract_id = () => command.ContractId.ShouldEqual(contractId);
    }
}