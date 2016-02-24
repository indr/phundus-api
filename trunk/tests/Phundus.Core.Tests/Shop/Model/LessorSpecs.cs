namespace Phundus.Tests.Shop.Model
{
    using Common.Domain.Model;
    using Common.Tests;
    using Machine.Specifications;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (Lessor))]
    public class when_serializing_a_lessor : serialization_object_concern<Lessor>
    {
        private static LessorId theLessorId;
        private static string theName;
        private static bool theDoesPublicRental;

        private Establish ctx = () =>
        {
            theLessorId = new LessorId();
            theName = "The lessors name";
            theDoesPublicRental = true;
            sut_factory.create_using(() =>
                new Lessor(theLessorId, theName, theDoesPublicRental));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_lessor_id = () =>
            dataMember(1).ShouldEqual(theLessorId.Id);

        private It should_have_at_2_the_name = () =>
            dataMember(2).ShouldEqual(theName);

        private It should_have_at_3_the_does_public_rental = () =>
            dataMember(3).ShouldEqual(theDoesPublicRental);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Orders.Model.Lessor");
    }
}