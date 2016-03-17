namespace Phundus.Tests.IdentityAccess.Model.Users
{
    using System;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Users;
    using Phundus.IdentityAccess.Users.Model;
    using Rhino.Mocks;

    public class user_concern : aggregate_concern<User>
    {
        protected static identityaccess_factory make;

        protected static Admin theAdmin;
        protected static UserId theUserId;
        protected static string theEmailAddress = "user@test.phundus.ch";
        protected static string thePassword = "1234";

        private Establish ctx = () =>
        {
            make = new identityaccess_factory(fake);

            theAdmin = make.Admin();
            theUserId = new UserId();
            sut_factory.create_using(() =>
                new User(theUserId, theEmailAddress, thePassword, "Hans", "Müller", "Street", "1234", "City",
                    "012 345 67 89", 123456));
        };
    }

    [Subject(typeof (User))]
    public class when_creating_a_user : user_concern
    {
        private It should_have_a_password = () =>
            sut.Account.Password.ShouldNotBeEmpty();

        private It should_have_role_user = () =>
            sut.Role.ShouldEqual(UserRole.User);

        private It should_have_the_city = () =>
            sut.City.ShouldEqual("City");

        private It should_have_the_email_address = () =>
            sut.Account.Email.ShouldEqual("user@test.phundus.ch");

        private It should_have_the_first_name = () =>
            sut.FirstName.ShouldEqual("Hans");

        private It should_have_the_js_number = () =>
            sut.JsNumber.ShouldEqual(123456);

        private It should_have_the_last_name = () =>
            sut.LastName.ShouldEqual("Müller");

        private It should_have_the_mobile_number = () =>
            sut.PhoneNumber.ShouldEqual("012 345 67 89");

        private It should_have_the_postcode = () =>
            sut.Postcode.ShouldEqual("1234");

        private It should_have_the_street = () =>
            sut.Street.ShouldEqual("Street");

        private It should_have_validation_key = () =>
            sut.Account.ValidationKey.ShouldNotBeEmpty();

        private It should_not_be_approved = () =>
            sut.Account.IsApproved.ShouldBeFalse();
    }

    [Subject(typeof (User))]
    public class when_validating_a_valid_account_validation_key : user_concern
    {
        private static string theKey;
        private static bool result;

        private Establish ctx = () =>
            sut_setup.run(sut =>
                theKey = sut.Account.ValidationKey);

        private Because of = () =>
            result = sut.Account.ValidateKey(theKey);

        private It should_be_approved = () =>
            sut.Account.IsApproved.ShouldBeTrue();

        private It should_not_have_a_validation_key = () =>
            sut.Account.ValidationKey.ShouldBeNull();

        private It should_return_true = () =>
            result.ShouldBeTrue();
    }

    [Subject(typeof (User))]
    public class when_validating_an_invalid_account_validation_key : user_concern
    {
        private static bool result;

        private Because of = () =>
            result = sut.Account.ValidateKey("wrongKey");

        private It should_not_approve = () =>
            sut.Account.IsApproved.ShouldBeFalse();

        private It should_not_remove_the_validation_key = () =>
            sut.Account.ValidationKey.ShouldNotBeEmpty();

        private It should_return_false = () =>
            result.ShouldBeFalse();
    }

    [Subject(typeof (User))]
    public class when_changing_address : user_concern
    {
        private static string theFirstName = "The new first name";
        private static string theLastName = "The new last name";
        private static string theStreet = "The new street";
        private static string thePostcode = "The new postcode";
        private static string theCity = "The new city";
        private static string thePhoneNumber = "The new phone number";

        private Because of = () =>
            sut.ChangeAddress(theInitiator, theFirstName, theLastName, theStreet, thePostcode, theCity, thePhoneNumber);

        private It should_have_the_city = () =>
            sut.City.ShouldEqual(theCity);

        private It should_have_the_first_name = () =>
            sut.FirstName.ShouldEqual(theFirstName);

        private It should_have_the_last_name = () =>
            sut.LastName.ShouldEqual(theLastName);

        private It should_have_the_phone_number = () =>
            sut.PhoneNumber.ShouldEqual(thePhoneNumber);

        private It should_have_the_postcode = () =>
            sut.Postcode.ShouldEqual(thePostcode);

        private It should_have_the_street = () =>
            sut.Street.ShouldEqual(theStreet);

        private It should_publish_user_address_changed = () =>
            published<UserAddressChanged>(p =>
                Equals(p.Initiator, theInitiator.ToActor())
                && p.UserId == theUserId.Id
                && p.FirstName == theFirstName
                && p.LastName == theLastName
                && p.Street == theStreet
                && p.Postcode == thePostcode
                && p.City == theCity
                && p.PhoneNumber == thePhoneNumber);
    }

    [Subject(typeof (User))]
    public class when_changing_email_address : user_concern
    {
        private static string theNewEmailAddress = "new@test.phundus.ch";
        private static string theOldValidationKey;

        private Establish ctx = () =>
            sut_setup.run(sut => theOldValidationKey = sut.Account.ValidationKey);

        private Because of = () =>
            sut.ChangeEmailAddress(theInitiator, thePassword, theNewEmailAddress);

        private It should_have_new_validation_key = () =>
            sut.Account.ValidationKey.ShouldNotEqual(theOldValidationKey);

        private It should_have_requested_email_address = () =>
            sut.Account.RequestedEmail.ShouldEqual(theNewEmailAddress);

        private It should_publish_email_address_change_requested = () => publisher.WasToldTo(
            x => x.Publish(Arg<EmailAddressChangeRequested>.Is.NotNull));
    }

    [Subject(typeof (User))]
    public class when_validating_a_valid_email_validation_key : user_concern
    {
        private static string theKey;
        private static bool result;
        private static string theNewEmailAddress = "new@test.phundus.ch";

        private Establish ctx = () =>
            sut_setup.run(sut =>
            {
                sut.Account.ChangeEmailAddress(theInitiator, thePassword, theNewEmailAddress);
                theKey = sut.Account.ValidationKey;
            });

        private Because of = () => result = sut.Account.ValidateKey(theKey);

        private It should_not_have_a_validation_key = () => sut.Account.ValidationKey.ShouldBeNull();

        private It should_publish_email_address_changed = () =>
        {
            publisher.WasToldTo(
                x => x.Publish(Arg<UserEmailAddressChanged>.Is.NotNull));
            publisher.WasToldTo(x => x.Publish(Arg<UserEmailAddressChanged>.Matches(p =>
                p.UserId == sut.UserId.Id
                && p.OldEmailAddress == theEmailAddress
                && p.NewEmailAddress == theNewEmailAddress)));
        };

        private It should_return_true = () =>
            result.ShouldBeTrue();

        private It should_set_new_email_address = () =>
            sut.Account.Email.ShouldEqual(theNewEmailAddress);
    }

    [Subject(typeof (User))]
    public class when_validating_an_invalid_email_validation_key : user_concern
    {
        private static string theNewEmailAddress = "new@test.phundus.ch";
        private static bool result;

        private Establish ctx = () =>
            sut_setup.run(sut => sut.Account.ChangeEmailAddress(theInitiator, thePassword, theNewEmailAddress));

        private Because of = () =>
            result = sut.Account.ValidateKey("wrongKey");

        private It should_not_change_the_email_address = () =>
            sut.Account.Email.ShouldEqual(theEmailAddress);

        private It should_not_remove_the_requested_email_address = () =>
            sut.Account.RequestedEmail.ShouldEqual(theNewEmailAddress);

        private It should_not_remove_the_validation_key = () =>
            sut.Account.ValidationKey.ShouldNotBeEmpty();

        private It should_return_false = () =>
            result.ShouldBeFalse();
    }

    [Subject(typeof (User))]
    public class when_locking : user_concern
    {
        private Because of = () =>
            sut.Lock(theAdmin);

        private It should_be_locked = () =>
            sut.Account.IsLockedOut.ShouldBeTrue();

        private It should_have_last_locked_out_date = () =>
            sut.Account.LastLockoutDate.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        private It should_publish_user_locked = () =>
            published<UserLocked>(p => p.InitiatorId == theAdmin.UserId.Id
                                       && p.LockedAtUtc == sut.Account.LastLockoutDate
                                       && p.UserId == sut.UserId.Id);
    }

    [Subject(typeof (User))]
    public class when_unlocking : user_concern
    {
        private Establish ctx = () =>
            sut_setup.run(sut =>
                sut.Lock(theAdmin));

        private Because of = () =>
            sut.Unlock(theAdmin);

        private It should_not_be_locked = () =>
            sut.Account.IsLockedOut.ShouldBeFalse();

        private It should_publish_user_unlocked = () =>
            published<UserUnlocked>(p => p.InitiatorId == theAdmin.UserId.Id
                                         && p.LockedAtUtc == sut.Account.LastLockoutDate
                                         && p.UserId == sut.UserId.Id);
    }
}