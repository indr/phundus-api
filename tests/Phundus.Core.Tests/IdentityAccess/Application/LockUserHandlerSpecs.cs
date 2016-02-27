namespace Phundus.Tests.IdentityAccess.Application
{
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Authorization;
    using Phundus.IdentityAccess.Users.Model;

    [Subject(typeof (LockUserHandler))]
    public class when_lock_user_is_handled : identityaccess_command_handler_concern<LockUser, LockUserHandler>
    {
        private static User theUser;

        private Establish ctx = () =>
        {
            theUser = make.User();
            userRepository.WhenToldTo(x => x.GetByGuid(theUser.UserId)).Return(theUser);

            command = new LockUser(theInitiatorId, theUser.UserId);
        };

        private It should_enforce_initiator_to_manage_users = () =>
            EnforcedInitiatorTo<ManageUsersAccessObject>();

        private It should_tell_user_to_lock = () =>
            theUser.WasToldTo(x => x.Lock(theInitiator));
    }

    [Subject(typeof (UnlockUserHandler))]
    public class when_unlock_user_is_handled : identityaccess_command_handler_concern<UnlockUser, UnlockUserHandler>
    {
        private static User theUser;

        private Establish ctx = () =>
        {
            theUser = make.User();
            userRepository.WhenToldTo(x => x.GetByGuid(theUser.UserId)).Return(theUser);

            theUser.Lock(theInitiator);

            command = new UnlockUser(theInitiatorId, theUser.UserId);
        };

        private It should_enforce_initiator_to_manage_users = () =>
            EnforcedInitiatorTo<ManageUsersAccessObject>();

        private It should_tell_user_to_unlock = () =>
            theUser.WasToldTo(x => x.Unlock(theInitiator));
    }
}