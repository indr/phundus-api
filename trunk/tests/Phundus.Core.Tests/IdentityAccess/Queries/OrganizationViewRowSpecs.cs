namespace Phundus.Tests.IdentityAccess.Queries
{
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Queries.QueryModels;

    [Subject(typeof (OrganizationViewRow))]
    public class when_getting_website_without_protocol : Observes<OrganizationViewRow>
    {
        private Establish ctx = () => sut_setup.run(x => 
            x.Website = "test.phundus.ch");

        private It should_have_http_prefixed = () =>
            sut.Website.ShouldStartWith("http://");
    }

    [Subject(typeof(OrganizationViewRow))]
    public class when_getting_empty_website : Observes<OrganizationViewRow>
    {
        private Establish ctx = () => sut_setup.run(x =>
            x.Website = "");

        private It should_not_have_http_prefixed = () =>
            sut.Website.ShouldNotContain("http://");
    }
}