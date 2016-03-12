namespace Phundus.Specs.Features.Organizations
{
    using System;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class MembershipSteps : AppStepsBase
    {
        private Relationship _relationship;
        private Guid _applicationId;

        public MembershipSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"I applied for membership")]
        public void GivenIAppliedForMembership()
        {
            var organization = Ctx.Organization;
            _applicationId = App.ApplyForMembership(Ctx.User, organization);
            Ctx.Organization = organization;
        }

        [Given(@"I applied for membership to ""(.*)""")]
        public void GivenIAppliedForMembershipTo(string alias)
        {
            var organization = Ctx.Organizations[alias];
            _applicationId = App.ApplyForMembership(Ctx.User, organization);
            Ctx.Organization = organization;
        }

        [Given(@"the membership application is rejected")]
        public void GivenTheMembershipApplicationIsRejected()
        {
            App.LogInAsRoot();
            App.RejectMembershipApplication(Ctx.Organization, _applicationId);
            App.DeleteSessionCookies();
        }

        [Given(@"the membership application is approved")]
        public void GivenTheMembershipApplicationIsApproved()
        {
            App.LogInAsRoot();
            App.ApproveMembershipApplication(Ctx.Organization, _applicationId);
            App.DeleteSessionCookies();
        }

        [When(@"I try to apply for membership")]
        public void WhenITryToApplyForMembership()
        {
            App.ApplyForMembership(Ctx.User, Ctx.Organization, assertHttpStatus: false);
        }

        [When(@"I try to get my relationship status")]
        public void WhenIGetMyMembershipStatus()
        {
            var organization = Ctx.Organization;
            _relationship = App.GetRelationshipStatus(Ctx.User, organization.OrganizationId).Result;
        }

        [Then(@"my relationship status is ""(.*)""")]
        public void ThenMyMembershipStatusIsRequested(string status)
        {
            var organizationId = Ctx.Organization.OrganizationId;
            
            Eventual.NoAssertionException(() =>
            {
                var relationship = App.GetRelationshipStatus(null, organizationId).Result;
                Assert.That(relationship.Status, Is.EqualTo(status));
            });
        }

    }
}