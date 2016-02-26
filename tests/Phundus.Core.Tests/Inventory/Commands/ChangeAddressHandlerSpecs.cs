﻿namespace Phundus.Tests.Inventory.Commands
{
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Stores.Commands;
    using Phundus.Inventory.Stores.Model;

    [Subject(typeof (ChangeAddressHandler))]
    public class when_handling_change_address : store_command_handler_concern<ChangeAddress, ChangeAddressHandler>
    {
        private static StoreAggregate theStore;
        private static string theNewAddress;

        private Establish ctx = () =>
        {
            theStore = make.StoreAggregate(theOwner);
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