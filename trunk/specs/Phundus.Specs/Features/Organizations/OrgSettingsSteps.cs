namespace Phundus.Specs.Features.Organizations
{
    using System;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class OrgSettingsSteps : AppStepsBase
    {
        private readonly ApiClient _api;

        public OrgSettingsSteps(App app, Ctx ctx, ApiClient api) : base(app, ctx)
        {
            if (api == null) throw new ArgumentNullException("api");
            _api = api;
        }

        [Given(@"I set organization setting public rental (on|off)")]
        [When(@"I try to set organization setting public rental (on|off)")]
        public void WhenITryToSetOrganizationSettingOn(bool value)
        {
            _api.OrganizationsSettingsApi.Patch(new
            {
                organizationId = Ctx.Organization.OrganizationId,
                publicRental = value
            });

            var organizationId = Ctx.Organization.OrganizationId;
            Eventual.NoTestException(() =>
            {
                var organization = App.GetOrganization(organizationId);
                Assert.That(organization.PublicRental, Is.EqualTo(value));
            });
        }

        [Then(@"should the organization settings equal")]
        public void ThenShouldTheOrganizationSettingsEqual(Table table)
        {
            var settings = _api.Assert(true).OrganizationsSettingsApi.Get<OrganizationsSettings>(new { organizationId = Ctx.Organization.OrganizationId }).Data;
            table.CompareToInstance(settings);
        }
    }
}