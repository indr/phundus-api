namespace Phundus.Tests.IdentityAccess.Users.Commands
{
    using System;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Users.Commands;
    using Phundus.IdentityAccess.Users.Model;
    using Phundus.IdentityAccess.Users.Repositories;
    using Rhino.Mocks;

    [Subject(typeof (ChangeEmailAddressHandler))]
    public class when_handling_change_email_address :
        identityaccess_handler_concern<ChangeEmailAddress, ChangeEmailAddressHandler>
    {
        private static string theNewEmailAddress = "new@test.phundus.ch";
        private static User theUser;

        private Establish ctx = () =>
        {
            theUser = make.User(theInitiatorId);
            depends.on<IUserRepository>().WhenToldTo(x => x.GetByGuid(theInitiatorId)).Return(theUser);
            command = new ChangeEmailAddress(theInitiatorId, "1234", theNewEmailAddress);
        };

        private It should_tell_to_change_email_address = () => theUser.WasToldTo(
            x =>
                x.ChangeEmailAddress(Arg<UserId>.Is.Equal(theInitiatorId), Arg<String>.Is.Equal("1234"),
                    Arg<String>.Is.Equal(theNewEmailAddress)));
    }
}