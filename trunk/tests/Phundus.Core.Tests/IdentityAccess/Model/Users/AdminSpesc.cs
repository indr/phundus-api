namespace Phundus.Tests.IdentityAccess.Model.Users
{
    using Common.Domain.Model;
    using Common.Tests;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Users;

    [Subject(typeof (Admin))]
    public class when_deserializing_an_admin : serialization_object_concern<Admin>
    {
        private static UserId userId = new UserId();
        private static string theEmailAddress = "admin@test.phundus.ch";
        private static string theFullName = "The Admin";

        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new Admin(userId, theEmailAddress, theFullName));

        private It should_have_at_1_the_user_id = () =>
            dataMember(1).ShouldEqual(userId.Id);

        private It should_have_at_2_the_email_address = () =>
            dataMember(2).ShouldEqual(theEmailAddress);

        private It should_have_at_3_the_full_name = () =>
            dataMember(3).ShouldEqual(theFullName);

        private It should_have_email_address = () =>
            sut.EmailAddress.ShouldEqual(theEmailAddress);

        private It should_have_full_name = () =>
            sut.FullName.ShouldEqual(theFullName);

        private It should_have_user_id = () =>
            sut.UserId.ShouldEqual(userId);
    }
}