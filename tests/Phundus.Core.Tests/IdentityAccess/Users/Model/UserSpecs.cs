namespace Phundus.Core.Tests.IdentityAndAccess.Users.Model
{
    using Core.IdentityAndAccess.Users.Model;
    using Machine.Specifications;

    [Subject(typeof (User))]
    public class create_user_specs
    {
        private static User sut;

        public Establish ctx = () => { };

        public Because of = () =>
        {
            sut = new User("user@test.phundus.ch", "1234", "Hans", "Müller", "Strasse", "1234", "Stadt", "012 345 67 89",
                null);
        };

        public It should_not_be_approved = () => sut.Account.IsApproved.ShouldBeFalse();
    }
}