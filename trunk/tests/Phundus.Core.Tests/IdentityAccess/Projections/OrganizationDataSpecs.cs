namespace Phundus.Tests.IdentityAccess.Projections
{
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Projections;

    [Subject(typeof (OrganizationData))]
    public class when_getting_website_without_protocol : Observes<OrganizationData>
    {
        private Establish ctx = () => sut_setup.run(x => 
            x.Website = "test.phundus.ch");

        private It should_have_http_prefixed = () =>
            sut.Website.ShouldStartWith("http://");
    }

    [Subject(typeof(OrganizationData))]
    public class when_getting_empty_website : Observes<OrganizationData>
    {
        private Establish ctx = () => sut_setup.run(x =>
            x.Website = "");

        private It should_not_have_http_prefixed = () =>
            sut.Website.ShouldNotContain("http://");
    }
}