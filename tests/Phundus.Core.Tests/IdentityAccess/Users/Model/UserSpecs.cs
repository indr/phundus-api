namespace Phundus.Core.Tests.IdentityAccess.Users.Model
{
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Users.Model;
    using Machine.Specifications;

    public class user_concern
    {
        protected static User sut;
        protected static string thePassword = "1234";

        protected static User CreateUser()
        {
            return new User("user@test.phundus.ch", thePassword, "Hans", "Müller", "Street", "1234", "City",
                "012 345 67 89",
                null);
        }
    }

    [Subject(typeof (User))]
    public class when_creating_a_user : user_concern
    {
        private Establish ctx = () => { };

        private Because of = () => { sut = CreateUser(); };

        private It should_have_role_user = () => sut.Role.ShouldEqual(UserRole.User);
        private It should_have_validation_key = () => sut.Account.ValidationKey.ShouldNotBeEmpty();
        private It should_not_be_approved = () => sut.Account.IsApproved.ShouldBeFalse();
    }

    [Subject(typeof (User))]
    public class when_validating_a_valid_account_validation_key : user_concern
    {
        private static string theKey;

        private Establish ctx = () =>
        {
            sut = CreateUser();
            theKey = sut.Account.ValidationKey;
        };

        private Because of = () => sut.Account.ValidateKey(theKey);

        private It should_be_approved = () => sut.Account.IsApproved.ShouldBeTrue();
        private It should_not_have_a_validation_key = () => sut.Account.ValidationKey.ShouldBeNull();
    }

    [Subject(typeof (User))]
    public class when_changing_email_address : user_concern
    {
        private static string theNewEmailAddress = "new@test.phundus.ch";

        private Establish ctx = () => { sut = CreateUser(); };

        private Because of = () => sut.Account.ChangeEmailAddress(thePassword, theNewEmailAddress);

        private It should_have_requested_email_address =
            () => sut.Account.RequestedEmail.ShouldEqual(theNewEmailAddress);

        private It should_have_validation_key = () => sut.Account.ValidationKey.ShouldNotBeEmpty();
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
            sut.Account.ChangeEmailAddress(thePassword, theNewEmailAddress);
            theKey = sut.Account.ValidationKey;
        };

        private Because of = () => result = sut.Account.ValidateKey(theKey);

        private It should_not_have_a_validation_key = () => sut.Account.ValidationKey.ShouldBeNull();
        private It should_return_true = () => result.ShouldBeTrue();
        private It should_set_new_email_address = () => sut.Account.Email.ShouldEqual(theNewEmailAddress);
    }
}