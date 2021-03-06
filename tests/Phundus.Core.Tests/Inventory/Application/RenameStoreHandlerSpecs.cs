﻿namespace Phundus.Tests.Inventory.Application
{
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Model.Stores;

    [Subject(typeof (RenameStoreHandler))]
    public class when_handling_rename_store : store_command_handler_concern<RenameStore, RenameStoreHandler>
    {
        private static Store theStore;
        private static string theNewName;

        private Establish ctx = () =>
        {
            theNewName = "The new address";

            theStore = make.Store(theOwner.OwnerId);
            storeRepository.setup(x => x.GetById(theStore.StoreId)).Return(theStore);

            command = new RenameStore(theInitiatorId, theStore.StoreId, theNewName);
        };

        private It should_save_to_repository = () =>
            storeRepository.received(x => x.Save(theStore));

        private It should_tell_store_to_rename = () =>
            theStore.received(x => x.Rename(theManager, theNewName));
    }
}