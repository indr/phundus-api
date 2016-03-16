namespace Phundus.Common.Tests.Domain.Model
{
    using System;
    using System.Reflection;
    using Common.Domain.Model;
    using Machine.Specifications;

    [Subject(typeof (Actor))]
    public class when_deserializing_an_actor : serialization_object_concern<Actor>
    {
        private static Guid actorGuid = Guid.NewGuid();

        private static PropertyInfo actorGuidProperty = typeof (Actor).GetProperty("ActorGuid",
            BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic);

        private static string theEmailAddress = "actor@test.phundus.ch";
        private static string theFullName = "The Actor";

        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new Actor(actorGuid, theEmailAddress, theFullName));

        private It should_have_at_1_the_guid = () =>
            dataMember(1).ShouldEqual(actorGuid);

        private It should_have_at_2_the_email_address = () =>
            dataMember(2).ShouldEqual(theEmailAddress);

        private It should_have_at_3_the_full_name = () =>
            dataMember(3).ShouldEqual(theFullName);

        private It should_have_the_email_address = () =>
            sut.EmailAddress.ShouldEqual(theEmailAddress);

        private It should_have_the_full_name = () =>
            sut.FullName.ShouldEqual(theFullName);

        private It should_have_the_guid = () =>
            actorGuidProperty.GetValue(sut, null).ShouldEqual(actorGuid);
    }
}