namespace Phundus.Tests.Inventory.Application
{
    using developwithpassion.specifications.extensions;    
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Model.Stores;
    using Rhino.Mocks;

    [Subject(typeof (ChangeContactDetailsHandler))]
    public class when_handling_change_contact_details :
        store_command_handler_concern<ChangeContactDetails, ChangeContactDetailsHandler>
    {
        private static Store theStore;

        private Establish ctx = () =>
        {
            theStore = make.Store(theOwnerId);
            storeRepository.setup(x => x.GetById(theStore.StoreId)).Return(theStore);

            command = new ChangeContactDetails(theInitiatorId, theStore.StoreId, "emailAddress", "phoneNumber", "line1",
                "line2", "street", "postcode", "city");
        };

        private It should_save_to_repository = () =>
            storeRepository.received(x => x.Save(theStore));

        private It should_tell_store_to_change_contact_details = () =>
            theStore.received(
                x => x.ChangeContactDetails(Arg<Manager>.Is.Equal(theManager), Arg<ContactDetails>.Is.NotNull));
    }
}