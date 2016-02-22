namespace Phundus.Tests.IdentityAccess.Model
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Model;

    public class membership_concern : Observes<Membership>
    {
        private Establish ctx = () => sut_factory.create_using(() =>
            new Membership(Guid.NewGuid(), new UserId(), Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
    }

    [Subject(typeof (Membership))]
    public class when_changing_role_to_manager : membership_concern
    {
        private Because of = () =>
            sut.ChangeRole(Role.Chief);

        private It should_have_recieves_email_notification_option = () =>
            sut.RecievesEmailNotifications.ShouldBeTrue();

        private It should_have_role_manager = () =>
            sut.Role.ShouldEqual(Role.Chief);
    }

    [Subject(typeof (Membership))]
    public class when_changing_role_to_member : membership_concern
    {
        private Establish ctx = () => sut_setup.run(sut =>
            sut.ChangeRole(Role.Chief));

        private Because of = () =>
            sut.ChangeRole(Role.Member);

        private It should_have_role_member = () =>
            sut.Role.ShouldEqual(Role.Member);

        private It should_not_have_recieves_email_notification_option = () =>
            sut.RecievesEmailNotifications.ShouldBeFalse();
    }
}