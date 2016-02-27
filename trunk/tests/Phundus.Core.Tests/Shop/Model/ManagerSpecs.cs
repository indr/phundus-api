namespace Phundus.Tests.Shop.Model
{
    using Common.Domain.Model;
    using Common.Tests;
    using Machine.Specifications;
    using Phundus.Shop.Model;

    [Subject(typeof (Manager))]
    public class when_serializing_a_manager : serialization_object_concern<Manager>
    {
        private static UserId theUserId;
        private static string theEmailAddress;
        private static string theFullName;

        private Establish ctx = () =>
        {
            theUserId = new UserId();
            theEmailAddress = "manager@test.phundus.ch";
            theFullName = "The Manager";
            sut_factory.create_using(() =>
                new Manager(theUserId, theEmailAddress, theFullName));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_user_id = () =>
            dataMember(1).ShouldEqual(theUserId.Id);

        private It should_have_at_2_the_email_address = () =>
            dataMember(2).ShouldEqual(theEmailAddress);

        private It should_have_at_3_the_full_name = () =>
            dataMember(3).ShouldEqual(theFullName);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Model.Manager");
    }
}