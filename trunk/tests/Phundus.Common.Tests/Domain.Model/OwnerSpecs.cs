namespace Phundus.Common.Tests.Domain.Model
{
    using System.IO;
    using Common.Domain.Model;
    using Machine.Specifications;
    using ProtoBuf;

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

        private Because of = () =>
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, sut);
            stream.Seek(0, SeekOrigin.Begin);
            sut = Serializer.Deserialize<Owner>(stream);
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Common");

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Common.Domain.Model.Owner");

        private It should_have_the_name_at_3 = () =>
            dataMember(3).ShouldEqual(theName);

        private It should_have_the_owner_id_at_1 = () =>
            dataMember(1).ShouldEqual(theOwnerId);

        private It should_have_the_type_at_2 = () =>
            dataMember(2).ShouldEqual(theType);
    }
}