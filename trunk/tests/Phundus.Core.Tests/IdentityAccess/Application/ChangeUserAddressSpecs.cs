namespace Phundus.Tests.IdentityAccess.Application
{
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Authorization;
    using Phundus.IdentityAccess.Users.Model;

    [Subject(typeof (ChangeUserAddressHandler))]
    public class when_handling_change_user_address :
        identityaccess_command_handler_concern<ChangeUserAddress, ChangeUserAddressHandler>
    {
        private static string theFirstName = "The first name";
        private static string theLastName = "The last name";
        private static string theStreet = "The street";
        private static string thePostcode = "The postcode";
        private static string theCity = "The city";
        private static string thePhoneNumber = "The phone number";
        private static User theUser;

        private Establish ctx = () =>
        {
            theUser = make.User();

            userRepository.setup(x => x.GetById(theUser.UserId)).Return(theUser);

            command = new ChangeUserAddress(theInitiatorId, theUser.UserId, theFirstName, theLastName, theStreet,
                thePostcode, theCity, thePhoneNumber);
        };

        private It should_change_user_address = () =>
            theUser.received(x =>
                x.ChangeAddress(theInitiator, theFirstName, theLastName, theStreet, thePostcode, theCity,
                    thePhoneNumber));

        private It should_enforce_initiator_to_manage_user = () =>
            enforceInitiatorTo<ManageUserAccessObject>(p =>
                Equals(p.UserId, theUser.UserId));

        private It should_save_to_repository = () =>
            userRepository.received(x => x.Save(theUser));
    }
}