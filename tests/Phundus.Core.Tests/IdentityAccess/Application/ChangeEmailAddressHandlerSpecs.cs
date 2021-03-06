﻿namespace Phundus.Tests.IdentityAccess.Application
{
    using System;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Model.Users;
    using Phundus.IdentityAccess.Users.Model;
    using Rhino.Mocks;

    [Subject(typeof (ChangeEmailAddressHandler))]
    public class when_handling_change_email_address :
        identityaccess_command_handler_concern<ChangeEmailAddress, ChangeEmailAddressHandler>
    {
        private static string theNewEmailAddress = "new@test.phundus.ch";
        private static User theUser;

        private Establish ctx = () =>
        {
            theUser = make.User(theInitiatorId);
            depends.on<IUserRepository>().WhenToldTo(x => x.GetById(theInitiatorId)).Return(theUser);
            command = new ChangeEmailAddress(theInitiatorId, theInitiatorId, "1234", theNewEmailAddress);
        };

        private It should_tell_to_change_email_address = () =>
            theUser.WasToldTo(x =>
                x.ChangeEmailAddress(Arg<Initiator>.Is.Equal(theInitiator), Arg<String>.Is.Equal("1234"),
                    Arg<String>.Is.Equal(theNewEmailAddress)));
    }
}