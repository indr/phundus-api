namespace Phundus.Common.Tests
{
    using System;
    using Domain.Model;
    using Machine.Specifications;

    [Subject(typeof (ProviderUserKey))]
    public class when_creating_with_id_string
    {
        private static ProviderUserKey sut;

        private Because of = () => sut = new ProviderUserKey("1234/3dc99e3e-25f1-4688-af0f-5083fc5c452b");

        private It should_have_user_id = () => sut.UserId.Id.ShouldEqual(1234);

        private It should_have_user_guid =
            () => sut.UserGuid.Id.ShouldEqual(new Guid("3dc99e3e-25f1-4688-af0f-5083fc5c452b"));

        private It should_have_to_string = () => sut.ToString().ShouldEqual("1234/3dc99e3e-25f1-4688-af0f-5083fc5c452b");
    }

    [Subject(typeof (ProviderUserKey))]
    public class when_creating_with_id_and_guid
    {
        private static ProviderUserKey sut;

        private Because of = () => sut = new ProviderUserKey(2345, new Guid("00fec87c-dda8-4f22-a4d7-eb1f6ba0c11b"));

        private It should_have_user_id = () => sut.UserId.Id.ShouldEqual(2345);

        private It shoud_have_user_guid =
            () => sut.UserGuid.Id.ShouldEqual(new Guid("00fec87c-dda8-4f22-a4d7-eb1f6ba0c11b"));

        private It should_have_to_string = () => sut.ToString().ShouldEqual("2345/00fec87c-dda8-4f22-a4d7-eb1f6ba0c11b");
    }
}