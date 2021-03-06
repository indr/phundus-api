﻿namespace Phundus.Tests.IdentityAccess.Application
{
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Users.Model;

    [Subject(typeof (LockUserHandler))]
    public class when_lock_user_is_handled : identityaccess_command_handler_concern<LockUser, LockUserHandler>
    {
        private static User theUser;

        private Establish ctx = () =>
        {
            theUser = make.User();
            userRepository.WhenToldTo(x => x.GetById(theUser.UserId)).Return(theUser);

            command = new LockUser(theInitiatorId, theUser.UserId);
        };

        private It should_lock_user = () =>
            theUser.WasToldTo(x => x.Lock(theAdmin));
    }
}