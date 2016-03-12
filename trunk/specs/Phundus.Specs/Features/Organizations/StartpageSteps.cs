namespace Phundus.Specs.Features.Organizations
{
    using System;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class StartpageSteps : AppStepsBase
    {
        private string _startpage;
        private string _theStartpage;

        public StartpageSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"I updated the startpage")]
        [When(@"I update the startpage")]
        public void GivenIUpdatedTheStartpage()
        {
            App.UpdateStartpage(Ctx.Organization, GenerateNewStartpage());
        }

        private string GenerateNewStartpage()
        {
            _theStartpage = string.Format("<h1>This is the new startpage</h1><p>{0}</p>", Guid.NewGuid().ToString("D"));
            return _theStartpage;
        }

        [When(@"I try to update the startpage")]
        public void WhenITryToUpdateTheStartpage()
        {
            App.UpdateStartpage(Ctx.Organization, GenerateNewStartpage());
        }

        [When(@"I try to get the startpage")]
        public void WhenITryToGetTheOrganizationDetails()
        {
            _startpage = App.GetOrganization(Ctx.Organization.OrganizationId).Startpage;
        }

        [Then(@"I should see the updated startpage")]
        public void ThenIShouldSeeTheUpdatedStartpage()
        {
            var organizationId = Ctx.Organization.OrganizationId;
            Eventual.NoAssertionException(() =>
            {
                var startpage = App.GetOrganization(organizationId).Startpage;
                Assert.That(startpage, Is.EqualTo(_theStartpage));    
            });
            
        }
    }
}