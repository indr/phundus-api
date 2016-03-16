namespace Phundus.Tests.IdentityAccess.Application
{
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Users.Model;

    [Subject(typeof (UnlockUserHandler))]
    public class when_unlock_user_is_handled : identityaccess_command_handler_concern<UnlockUser, UnlockUserHandler>
    {
        private static User theUser;

        private Establish ctx = () =>
        {
            theUser = make.User();
            userRepository.WhenToldTo(x => x.GetById(theUser.UserId)).Return(theUser);

            theUser.Lock(theAdmin);

            command = new UnlockUser(theInitiatorId, theUser.UserId);
        };

        private It should_unlock_user = () =>
            theUser.WasToldTo(x => x.Unlock(theAdmin));
    }
}