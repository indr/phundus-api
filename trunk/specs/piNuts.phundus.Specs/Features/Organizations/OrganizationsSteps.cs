namespace Phundus.Specs.Features.Organizations
{
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class OrganizationsSteps : StepsBase
    {
        public OrganizationsSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"establish organization")]
        public void WhenEstablishOrganization()
        {
            var organization = App.EstablishOrganization();
            Ctx.Organization = organization;
        }

        [Then(@"query organizations should contain it")]
        public void ThenQueryOrganizationsShouldContainIt()
        {
            var organizations = App.QueryOrganizations();
            Assert.That(organizations, Has.Some.Matches<Phundus.Rest.ContentObjects.Organization>(p => p.OrganizationId == Ctx.Organization.Guid));
        }
    }
}