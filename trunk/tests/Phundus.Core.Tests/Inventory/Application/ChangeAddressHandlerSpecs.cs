namespace Phundus.Tests.Inventory.Application
{
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Model.Stores;

    [Subject(typeof (ChangeAddressHandler))]
    public class when_handling_change_address : store_command_handler_concern<ChangeAddress, ChangeAddressHandler>
    {
        private static Store theStore;
        private static string theNewAddress;

        private Establish ctx = () =>
        {
            theStore = make.Store(theOwner.OwnerId);
            theNewAddress = "The new address";
            storeRepository.setup(x => x.GetById(theStore.StoreId)).Return(theStore);

            command = new ChangeAddress(theInitiatorId, theStore.StoreId, theNewAddress);
        };

        private It should_save_to_repository = () =>
            storeRepository.received(x => x.Save(theStore));

        private It should_tell_store_to_change_address = () =>
            theStore.received(x => x.ChangeAddress(theManager, theNewAddress));
    }
}