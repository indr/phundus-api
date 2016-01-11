namespace Phundus.Specs.Features.Organizations
{
    using System.Collections.Generic;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class OrganizationsSteps : StepsBase
    {
        private Organization _organizationDetails;
        private IList<Organization> _organizations;

        public OrganizationsSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"I established an organization")]
        public void GivenIEstablishedAnOrganization()
        {
            var organization = App.EstablishOrganization();
            Ctx.Organization = organization;
        }

        [When(@"I try to establish an organization")]
        public void WhenITryToEstablishAnOrganization()
        {
            var organization = App.EstablishOrganization();
            Ctx.Organization = organization;
        }

        [When(@"I try to get the organization details")]
        public void WhenITryToGetTheOrganizationDetails()
        {
            _organizationDetails = App.GetOrganization(Ctx.Organization.OrganizationId);
        }

        [When(@"I try to query all organizations")]
        public void WhenITryToQueryAllOrganizations()
        {
            _organizations = App.QueryOrganizations();
        }

        [Then(@"I should find the organization in the result")]
        public void ThenIShouldFindTheOrganizationInTheResult()
        {
            Assert.That(_organizations,
                Has.Some.Matches<Organization>(p => p.OrganizationId == Ctx.Organization.OrganizationId));
        }

        [Then(@"I should see the organization details")]
        public void ThenIShouldSeeTheOrganizationDetails()
        {
            Assert.That(_organizationDetails, Is.Not.Null);
            Assert.That(_organizationDetails.OrganizationId, Is.EqualTo(Ctx.Organization.OrganizationId));
        }
    }
}