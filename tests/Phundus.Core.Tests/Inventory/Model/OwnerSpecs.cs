namespace Phundus.Tests.Inventory.Model
{
    using System;
    using Common.Domain.Model;
    using Common.Tests;
    using Machine.Specifications;
    using Phundus.Inventory.Model;

    [Subject(typeof (Owner))]
    public class when_instantiating_with_owner_type_unknown
    {
        private static Exception caughtException;

        private Because of = () =>
            caughtException = Catch.Exception(() => new Owner(new OwnerId(), "The name", OwnerType.Unknown));

        private It should_throw_exception_message_starting_with = () =>
            caughtException.Message.ShouldStartWith("Owner type must not be unknown.");

        private It should_throw_invalid_operation_exception = () =>
            caughtException.ShouldBeOfExactType<ArgumentException>();
    }

    [Subject(typeof (Owner))]
    public class when_serializing_an_owner : serialization_object_concern<Owner>
    {
        private static OwnerId theOwnerId;
        private static string theName;
        private static OwnerType theType;

        private Establish ctx = () =>
        {
            theOwnerId = new OwnerId();
            theName = "The owner";
            theType = OwnerType.Organization;
            sut_factory.create_using(() =>
                new Owner(theOwnerId, theName, theType));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_owner_id = () =>
            dataMember(1).ShouldEqual(theOwnerId.Id);

        private It should_have_at_2_the_type = () =>
            dataMember(2).ShouldEqual(theType);

        private It should_have_at_3_the_name = () =>
            dataMember(3).ShouldEqual(theName);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Model.Owner");
    }
}