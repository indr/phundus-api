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

        [Then(@"get organization details")]
        public void ThenGetOrganizationDetails()
        {
            var organization = App.GetOrganization(Ctx.Organization.Guid);
            Assert.That(organization, Is.Not.Null);
            Assert.That(organization.OrganizationGuid, Is.EqualTo(Ctx.Organization.Guid));
        }
    }
}