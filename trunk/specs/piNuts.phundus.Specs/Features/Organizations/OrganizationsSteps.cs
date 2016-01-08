namespace Phundus.Specs.Steps
{
    using NUnit.Framework;
    using Services;
    using TechTalk.SpecFlow;

    [Binding]
    public class OrganizationsSteps : StepsBase
    {
        public OrganizationsSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"establish organization")]
        public void WennEstablishOrganization()
        {
            var organization = App.EstablishOrganization();
            Ctx.CurrentOrganization = organization;
        }

        [Then(@"query organizations should contain it")]
        public void DannQueryOrganizationsShouldContainIt()
        {
            var organizations = App.QueryOrganizations();
            Assert.That(organizations, Has.Some.Matches<Phundus.Rest.ContentObjects.Organization>(p => p.OrganizationId == Ctx.CurrentOrganization.Guid));
        }
    }
}