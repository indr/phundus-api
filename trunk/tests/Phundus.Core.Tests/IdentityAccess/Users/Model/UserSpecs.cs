namespace Phundus.Tests.IdentityAccess.Users.Model
{
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Users.Model;
    using Rhino.Mocks;

    public class user_concern : aggregate_concern<User>
    {
        protected static string theEmailAddress = "user@test.phundus.ch";
        protected static string thePassword = "1234";

        protected static User CreateUser()
        {
            return new User(theEmailAddress, thePassword, "Hans", "Müller", "Street", "1234", "City",
                "012 345 67 89", 123456);
        }
    }

    [Subject(typeof (User))]
    public class when_creating_a_user : user_concern
    {
        private Because of = () => { sut = CreateUser(); };

        private It should_have_a_password = () => sut.Account.Password.ShouldNotBeEmpty();
        private It should_have_role_user = () => sut.Role.ShouldEqual(UserRole.User);
        private It should_have_the_city = () => sut.City.ShouldEqual("City");
        private It should_have_the_email_address = () => sut.Account.Email.ShouldEqual("user@test.phundus.ch");
        private It should_have_the_first_name = () => sut.FirstName.ShouldEqual("Hans");
        private It should_have_the_js_number = () => sut.JsNumber.ShouldEqual(123456);
        private It should_have_the_last_name = () => sut.LastName.ShouldEqual("Müller");
        private It should_have_the_mobile_number = () => sut.MobileNumber.ShouldEqual("012 345 67 89");
        private It should_have_the_postcode = () => sut.Postcode.ShouldEqual("1234");
        private It should_have_the_street = () => sut.Street.ShouldEqual("Street");
        private It should_have_validation_key = () => sut.Account.ValidationKey.ShouldNotBeEmpty();
        private It should_not_be_approved = () => sut.Account.IsApproved.ShouldBeFalse();
    }

    [Subject(typeof (User))]
    public class when_validating_a_valid_account_validation_key : user_concern
    {
        private static string theKey;
        private static bool result;

        private Establish ctx = () =>
        {
            sut = CreateUser();
            theKey = sut.Account.ValidationKey;
        };

        private Because of = () => result = sut.Account.ValidateKey(theKey);

        private It should_be_approved = () => sut.Account.IsApproved.ShouldBeTrue();
        private It should_not_have_a_validation_key = () => sut.Account.ValidationKey.ShouldBeNull();
        private It should_return_true = () => result.ShouldBeTrue();
    }

    [Subject(typeof (User))]
    public class when_validating_an_invalid_account_validation_key : user_concern
    {
        private static bool result;
        private Establish ctx = () => sut = CreateUser();

        private Because of = () => result = sut.Account.ValidateKey("wrongKey");

        private It should_not_approve = () => sut.Account.IsApproved.ShouldBeFalse();
        private It should_not_remove_the_validation_key = () => sut.Account.ValidationKey.ShouldNotBeEmpty();
        private It should_return_false = () => result.ShouldBeFalse();
    }

    [Subject(typeof (User))]
    public class when_changing_email_address : user_concern
    {
        private static string theNewEmailAddress = "new@test.phundus.ch";
        private static string theOldValidationKey;

        private Establish ctx = () =>
        {
            sut = CreateUser();
            theOldValidationKey = sut.Account.ValidationKey;
        };

        private Because of = () => sut.ChangeEmailAddress(sut.UserGuid, thePassword, theNewEmailAddress);

        private It should_have_new_validation_key = () => sut.Account.ValidationKey.ShouldNotEqual(theOldValidationKey);

        private It should_have_requested_email_address =
            () => sut.Account.RequestedEmail.ShouldEqual(theNewEmailAddress);

        private It should_public_email_address_change_requested = () => publisher.WasToldTo(
            x => x.Publish(Arg<UserEmailAddressChangeRequested>.Is.NotNull));
    }

    [Subject(typeof (User))]
    public class when_validating_a_valid_email_validation_key : user_concern
    {
        private static string theKey;
        private static bool result;
        private static string theNewEmailAddress = "new@test.phundus.ch";

        private Establish ctx = () =>
        {
            sut = CreateUser();
            sut.Account.ChangeEmailAddress(sut.UserGuid, thePassword, theNewEmailAddress);
            theKey = sut.Account.ValidationKey;
        };

        private Because of = () => result = sut.Account.ValidateKey(theKey);

        private It should_not_have_a_validation_key = () => sut.Account.ValidationKey.ShouldBeNull();

        private It should_publish_email_address_changed = () =>
        {
            publisher.WasToldTo(
                x => x.Publish(Arg<UserEmailAddressChanged>.Is.NotNull));
            publisher.WasToldTo(x => x.Publish(Arg<UserEmailAddressChanged>.Matches(p =>
                p.UserGuid == sut.UserGuid.Id
                && p.OldEmailAddress == theEmailAddress
                && p.NewEmailAddress == theNewEmailAddress)));
        };

        private It should_return_true = () => result.ShouldBeTrue();
        private It should_set_new_email_address = () => sut.Account.Email.ShouldEqual(theNewEmailAddress);
    }

    [Subject(typeof (User))]
    public class when_validating_an_invalid_email_validation_key : user_concern
    {
        private static string theNewEmailAddress = "new@test.phundus.ch";
        private static bool result;

        private Establish ctx = () =>
        {
            sut = CreateUser();
            sut.Account.ChangeEmailAddress(sut.UserGuid, thePassword, theNewEmailAddress);
        };

        private Because of = () => result = sut.Account.ValidateKey("wrongKey");

        private It should_not_change_the_email_address = () => sut.Account.Email.ShouldEqual(theEmailAddress);

        private It should_not_remove_the_requested_email_address =
            () => sut.Account.RequestedEmail.ShouldEqual(theNewEmailAddress);

        private It should_not_remove_the_validation_key = () => sut.Account.ValidationKey.ShouldNotBeEmpty();

        private It should_return_false = () => result.ShouldBeFalse();
    }
}