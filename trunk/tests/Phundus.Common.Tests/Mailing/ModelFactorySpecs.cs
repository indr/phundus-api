namespace Phundus.Common.Tests.Mailing
{
    using Common.Mailing;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class model_factory_concern : Observes<ModelFactory>
    {
        protected static object model;

        private Establish ctx = () => { };
    }

    public class TestModelFactoryData
    {
        public string Value { get; set; }
    }

    [Subject(typeof (ModelFactory))]
    public class when_making_model_with_null : model_factory_concern
    {
        private Because of = () =>
            model = sut.MakeModel(null);

        private It should_return_a_model_with_urls = () =>
            model.ShouldContainMemberOfExactType<Urls>("Urls");
    }

    [Subject(typeof (ModelFactory))]
    public class when_making_model_with_data : model_factory_concern
    {
        private static TestModelFactoryData data;
        private Establish ctx = () => { data = new TestModelFactoryData(); };

        private Because of = () =>
            model = sut.MakeModel(data);

        private It should_return_a_model_with_members_of_data = () =>
            model.ShouldContainMember("Value");
    }
}