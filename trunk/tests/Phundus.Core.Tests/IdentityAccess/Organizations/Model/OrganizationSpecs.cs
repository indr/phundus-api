﻿namespace Phundus.Tests.IdentityAccess.Organizations.Model
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Users.Model;
    using Rhino.Mocks;
    using ApplicationId = Common.Domain.Model.ApplicationId;

    public class organization_concern : aggregate_concern<Organization>
    {
        protected static InitiatorGuid theInitiatorId;

        private Establish ctx = () =>
        {
            theInitiatorId = new InitiatorGuid();
            sut = new Organization(Guid.NewGuid(), "Organization name");
        };

        protected static User CreateUser()
        {
            return new User("user@test.phundus.ch", "1234", "Hans", "Müller", "Street", "1000", "City", "012 345 67 89",
                null);
        }
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
        private static ApplicationId theApplicationId;
        private static User theUser;

        private Establish ctx = () =>
        {
            theApplicationId = new ApplicationId();
            theUser = CreateUser();
        };

        private Because of = () => sut.RequestMembership(theInitiatorId, theApplicationId, theUser);

        private It should_have_membership_application =
            () => sut.Applications.ShouldContain(e => Equals(e.ApplicationId, theApplicationId));

        private It should_publish_membership_application_filed =
            () => publisher.AssertWasCalled(x => x.Publish(Arg<MembershipApplicationFiled>.Is.NotNull));
    }

    [Subject(typeof (Organization))]
    public class when_requesting_membership_twice : organization_concern
    {
        private static ApplicationId theFirstApplicationId;
        private static ApplicationId theSecondApplicationId;
        private static User theUser;

        private Establish ctx = () =>
        {
            theFirstApplicationId = new ApplicationId();
            theSecondApplicationId = new ApplicationId();
            theUser = CreateUser();
            sut.RequestMembership(theInitiatorId, theFirstApplicationId, theUser);
        };

        private Because of = () => sut.RequestMembership(theInitiatorId, theSecondApplicationId, theUser);

        private It should_not_have_a_second_application = 
            () => sut.Applications.Count.ShouldEqual(1);

        private It should_not_publish_membership_application_filed =
            () => publisher.AssertWasCalled(x => x.Publish(Arg<MembershipApplicationFiled>.Is.Anything), x => x.Repeat.Once());
    }
}