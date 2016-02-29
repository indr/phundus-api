namespace Phundus.Tests.IdentityAccess.Model
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model;
    using Phundus.IdentityAccess.Model.Organizations;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Users.Model;
    using Rhino.Mocks;

    public class organization_concern : aggregate_concern<Organization>
    {
        protected static identityaccess_factory make;

        protected static OrganizationId theOrganizationId;
        protected static Admin theAdmin;
        protected static Manager theManager;

        private Establish ctx = () =>
        {
            make = new identityaccess_factory(fake);

            theInitiatorId = new InitiatorId();
            theInitiator = new Initiator(theInitiatorId, "initiator@test.phundus.ch", "The Initiator");
            theAdmin = new Admin(theInitiatorId, "admin@test.phundus.ch", "The Admin");
            theManager = new Manager(theInitiatorId, "manager@test.phundus.ch", "The Manager");
            theOrganizationId = new OrganizationId();

            sut_factory.create_using(() =>
                new Organization(theOrganizationId, "The organization"));
        };
    }

    [Subject(typeof (Organization))]
    public class when_establishing_an_organization : organization_concern
    {
        private Establish ctx = () => sut_factory.create_using(() =>
            new Organization(theOrganizationId, "The organization"));

        private It should_have_established_at_utc = () =>
            sut.EstablishedAtUtc.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        private It should_have_friendly_url = () =>
            sut.FriendlyUrl.ShouldEqual("the-organization");

        private It should_have_organization_plan_free = () =>
            sut.Plan.ShouldEqual(OrganizationPlan.Free);

        private It should_have_setting_public_rental_true = () =>
            sut.Settings.PublicRental.ShouldBeTrue();
    }

    [Subject(typeof (Organization))]
    public class when_changing_contact_details : organization_concern
    {
        private static ContactDetails theContactDetails;

        private Establish ctx = () =>
            theContactDetails = new ContactDetails("Line1", "Line2", "Street", "Postcode", "City", "Phone number", "Email address", "Website");

        private Because of = () =>
            sut.ChangeContactDetails(theInitiator, theContactDetails);

        private It should_publish_organization_contact_details_changed = () =>
            published<OrganizationContactDetailsChanged>(p =>
                Equals(p.Initiator, theInitiator)
                && p.OrganizationId == theOrganizationId.Id
                && p.Line1 == "Line1"
                && p.Line2 == "Line2"
                && p.Street == "Street"
                && p.Postcode == "Postcode"
                && p.City == "City"
                && p.PhoneNumber == "Phone number"
                && p.EmailAddress == "Email address"
                && p.Website == "Website");

        private It should_update_contact_details = () =>
            sut.ContactDetails.ShouldEqual(theContactDetails);
    }

    [Subject(typeof (Organization))]
    public class when_changing_the_organization_plan : organization_concern
    {
        private static OrganizationPlan theNewOrganizationPlan;

        private Establish ctx = () => { theNewOrganizationPlan = OrganizationPlan.Membership; };

        private Because of = () =>
            sut.ChangePlan(theAdmin, theNewOrganizationPlan);

        private It should_have_the_new_plan = () =>
            sut.Plan.ShouldEqual(theNewOrganizationPlan);

        private It should_public_organization_plan_changed = () =>
            published<OrganizationPlanChanged>(p =>
                p.Initiator.InitiatorId.Id == theAdmin.UserId.Id
                && p.OrganizationId == theOrganizationId.Id
                && p.OldPlan == "free"
                && p.NewPlan == theNewOrganizationPlan.ToString().ToLowerInvariant());
    }

    [Subject(typeof (Organization))]
    public class when_setting_members_recieves_email_notification : organization_concern
    {
        private static User theMember;
        private static Membership theMembership;

        private Establish ctx = () => sut_setup.run(sut =>
        {
            theMember = make.User();
            var membershipApplicationId = new MembershipApplicationId();
            var application = sut.ApplyForMembership(theInitiatorId, membershipApplicationId, theMember);
            sut.ApproveMembershipApplication(theInitiatorId, application, Guid.NewGuid());
            theMembership = sut.Memberships.Single(p => p.UserId.Id == theMember.UserId.Id);
        });

        private Because of = () =>
            sut.ChangeMembersRecieveEmailNotificationOption(theManager, theMember.UserId, true);

        private It should_have_membership_with_recieves_email_notification_option = () =>
            theMembership.RecievesEmailNotifications.ShouldBeTrue();

        private It should_publish_member_recieve_email_notification_changed = () =>
            published<MemberRecieveEmailNotificationOptionChanged>(p =>
                p.Initiator.InitiatorId.Id == theManager.UserId.Id
                && p.OrganizationId == theOrganizationId.Id
                && p.MemberId == theMember.UserId.Id
                && p.Value == true);
    }

    [Subject(typeof (Organization))]
    public class when_changing_startpage : organization_concern
    {
        private static string theNewStartpage = "<p>The new startpage</p>";

        private Because of = () =>
            sut.ChangeStartpage(theInitiator, theNewStartpage);

        private It should_have_new_startpage = () =>
            sut.Startpage.ShouldEqual(theNewStartpage);

        private It should_publish_startpage_changed = () =>
            published<StartpageChanged>(p =>
                Equals(p.Initiator, theInitiator)
                && p.OrganizationId == theOrganizationId.Id
                && p.Startpage == theNewStartpage);            
    }

    [Subject(typeof (Organization))]
    public class when_applying_for_membership : organization_concern
    {
        private static MembershipApplicationId theApplicationId;
        private static User theUser;

        private Establish ctx = () =>
        {
            theApplicationId = new MembershipApplicationId();
            theUser = make.User();
        };

        private Because of = () =>
            sut.ApplyForMembership(theInitiatorId, theApplicationId, theUser);

        private It should_have_membership_application =
            () => sut.Applications.ShouldContain(e => Equals(e.MembershipApplicationId, theApplicationId));

        private It should_publish_membership_application_filed =
            () => publisher.AssertWasCalled(x => x.Publish(Arg<MembershipApplicationFiled>.Is.NotNull));
    }

    [Subject(typeof (Organization))]
    public class when_applying_for_membership_twice : organization_concern
    {
        private static MembershipApplicationId theFirstApplicationId;
        private static MembershipApplicationId theSecondApplicationId;
        private static User theUser;

        private Establish ctx = () =>
        {
            theFirstApplicationId = new MembershipApplicationId();
            theSecondApplicationId = new MembershipApplicationId();
            theUser = make.User();

            sut_setup.run(sut =>
                sut.ApplyForMembership(theInitiatorId, theFirstApplicationId, theUser));
        };

        private Because of = () =>
            sut.ApplyForMembership(theInitiatorId, theSecondApplicationId, theUser);

        private It should_not_have_a_second_application =
            () => sut.Applications.Count.ShouldEqual(1);

        private It should_not_publish_membership_application_filed =
            () =>
                publisher.AssertWasCalled(x => x.Publish(Arg<MembershipApplicationFiled>.Is.Anything),
                    x => x.Repeat.Once());
    }
}