﻿namespace Phundus.Specs.Features.Organizations
{
    using System;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class MembershipSteps : StepsBase
    {
        private OrganizationsRelationshipsQueryOkResponseContent _relationship;
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

        [When(@"I try to get my relationship status")]
        public void WhenIGetMyMembershipStatus()
        {
            var organization = Ctx.Organization;
            _relationship = App.GetRelationshipStatus(Ctx.User, organization);
        }

        [Then(@"my relationship status is ""(.*)""")]
        public void ThenMyMembershipStatusIsRequested(string status)
        {
            Assert.That(_relationship.Status, Is.EqualTo(status));
        }

    }
}