namespace Phundus.Tests.Shop.Model
{
    using Common.Domain.Model;
    using Common.Tests;
    using Machine.Specifications;
    using Phundus.Shop.Model;

    [Subject(typeof (Manager))]
    public class when_deserializing_a_manager : serialization_object_concern<Manager>
    {
        private static UserId theUserId = new UserId();
        private static string theEmailAddress = "manager@test.phundus.ch";
        private static string theFullName = "The Manager";

        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new Manager(theUserId, theEmailAddress, theFullName));

        private It should_have_at_1_the_user_id = () =>
            dataMember(1).ShouldEqual(theUserId.Id);

        private It should_have_at_2_the_email_address = () =>
            dataMember(2).ShouldEqual(theEmailAddress);

        private It should_have_at_3_the_full_name = () =>
            dataMember(3).ShouldEqual(theFullName);

        private It should_have_the_email_address = () =>
            sut.EmailAddress.ShouldEqual(theEmailAddress);

        private It should_have_the_full_name = () =>
            sut.FullName.ShouldEqual(theFullName);

        private It should_have_the_user_id = () =>
            sut.UserId.ShouldEqual(theUserId);
    }
}