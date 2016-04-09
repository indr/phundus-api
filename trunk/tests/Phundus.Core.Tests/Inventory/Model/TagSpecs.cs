namespace Phundus.Tests.Inventory.Model
{
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Inventory.Model.Articles;

    public class tag_concern : Observes<Tag>
    {
    }

    [Subject(typeof (Tag))]
    public class creating_a_tag_sanitizes_name : tag_concern
    {
        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new Tag("Tä g@ 1"));

        private It should_have_url_friendly_name = () =>
            sut.Name.ShouldEqual("tä-g-1");
    }

    [Subject(typeof (Tag))]
    public class when_comparing_hash_code_of_two_tags_with_different_cases : tag_concern
    {
        private static Tag other = new Tag("ABC");

        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new Tag("abc"));

        private It should_have_the_same_hash_code = () =>
            sut.GetHashCode().ShouldEqual(other.GetHashCode());
    }
}