namespace Phundus.Tests.IdentityAccess.Organizations.Model
{
    using System;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Users.Model;
    using Rhino.Mocks;

    public class organization_concern : aggregate_concern<Organization>
    {
        protected static InitiatorId theInitiatorId;

        private Establish ctx = () =>
        {
            theInitiatorId = new InitiatorId();
            sut = new Organization(Guid.NewGuid(), "Organization name");
        };

        protected static User CreateUser()
        {
            return new User("user@test.phundus.ch", "1234", "Hans", "Müller", "Street", "1000", "City", "012 345 67 89",
                null);
        }
    }

    [Subject(typeof (Organization))]
    public class when_changing_contact_details : organization_concern
    {
        private static ContactDetails theContactDetails;

        private Establish ctx = () =>
            theContactDetails = new ContactDetails("Post address", "Phone number", "Email address", "Website");

        private Because of = () =>
            sut.ChangeContactDetails(theContactDetails);

        private It should_update_contact_details = () =>
            sut.ContactDetails.ShouldEqual(theContactDetails);

        private It should_publish_organization_contact_details_changed = () =>
            publisher.WasToldTo(x => x.Publish(Arg<OrganizationContactDetailsChanged>.Is.NotNull));
    }

    [Subject(typeof (Organization))]
    public class when_changing_startpage : organization_concern
    {
        private static string theNewStartpage = "<p>The new startpage</p>";

        private Because of = () => sut.ChangeStartpage(theInitiatorId, theNewStartpage);

        private It should_have_new_startpage = () => sut.Startpage.ShouldEqual(theNewStartpage);

        private It should_publish_startpage_changed =
            () => publisher.AssertWasCalled(x => x.Publish(Arg<StartpageChanged>.Is.NotNull));
    }

    [Subject(typeof (Organization))]
    public class when_requesting_membership : organization_concern
    {
        private static MembershipApplicationId theApplicationId;
        private static User theUser;

        private Establish ctx = () =>
        {
            theApplicationId = new MembershipApplicationId();
            theUser = CreateUser();
        };

        private Because of = () => sut.RequestMembership(theInitiatorId, theApplicationId, theUser);

        private It should_have_membership_application =
            () => sut.Applications.ShouldContain(e => Equals(e.MembershipApplicationId, theApplicationId));

        private It should_publish_membership_application_filed =
            () => publisher.AssertWasCalled(x => x.Publish(Arg<MembershipApplicationFiled>.Is.NotNull));
    }

    [Subject(typeof (Organization))]
    public class when_requesting_membership_twice : organization_concern
    {
        private static MembershipApplicationId theFirstApplicationId;
        private static MembershipApplicationId theSecondApplicationId;
        private static User theUser;

        private Establish ctx = () =>
        {
            theFirstApplicationId = new MembershipApplicationId();
            theSecondApplicationId = new MembershipApplicationId();
            theUser = CreateUser();
            sut.RequestMembership(theInitiatorId, theFirstApplicationId, theUser);
        };

        private Because of = () => sut.RequestMembership(theInitiatorId, theSecondApplicationId, theUser);

        private It should_not_have_a_second_application =
            () => sut.Applications.Count.ShouldEqual(1);

        private It should_not_publish_membership_application_filed =
            () =>
                publisher.AssertWasCalled(x => x.Publish(Arg<MembershipApplicationFiled>.Is.Anything),
                    x => x.Repeat.Once());
    }
}