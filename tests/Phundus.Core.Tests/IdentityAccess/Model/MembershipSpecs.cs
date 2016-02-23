namespace Phundus.Tests.IdentityAccess.Model
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Integration.IdentityAccess;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Model;

    public class membership_concern : Observes<Membership>
    {
        protected static identityaccess_factory make;

        protected static Exception caughtException;

        protected static UserId theMemberId;

        private Establish ctx = () =>
        {
            make = new identityaccess_factory(fake);

            theMemberId = new UserId();

            sut_factory.create_using(() =>
                new Membership(Guid.NewGuid(), theMemberId, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
        };
    }

    [Subject(typeof (Membership))]
    public class when_changing_role_to_manager : membership_concern
    {
        private Because of = () =>
            sut.ChangeRole(MemberRole.Manager);

        private It should_have_recieves_email_notification_option = () =>
            sut.RecievesEmailNotifications.ShouldBeTrue();

        private It should_have_role_manager = () =>
            sut.MemberRole.ShouldEqual(MemberRole.Manager);
    }

    [Subject(typeof (Membership))]
    public class when_changing_role_to_member : membership_concern
    {
        private Establish ctx = () => sut_setup.run(sut =>
            sut.ChangeRole(MemberRole.Manager));

        private Because of = () =>
            sut.ChangeRole(MemberRole.Member);

        private It should_have_role_member = () =>
            sut.MemberRole.ShouldEqual(MemberRole.Member);

        private It should_not_have_recieves_email_notification_option = () =>
            sut.RecievesEmailNotifications.ShouldBeFalse();
    }

    [Subject(typeof (Membership))]
    public class when_locking : membership_concern
    {
        private static Manager theManager;

        private Establish ctx = () =>
            theManager = make.Manager();

        private Because of = () =>
            sut.Lock(theManager);

        private It should_be_locked = () =>
            sut.IsLocked.ShouldBeTrue();
    }

    [Subject(typeof (Membership))]
    public class when_trying_to_lock_own_membership : membership_concern
    {
        private static Manager theManager;

        private Establish ctx = () =>
            theManager = make.Manager(theMemberId);

        private Because of = () =>
            caughtException = Catch.Exception(() => sut.Lock(theManager));

        private It should_throw_invalid_operation_exception = () =>
            caughtException.ShouldBeOfExactType<InvalidOperationException>();
    }

    [Subject(typeof (Membership))]
    public class when_unlocking : membership_concern
    {
        private static Manager theManager;

        private Establish ctx = () =>
        {
            theManager = make.Manager();
            sut_setup.run(sut =>
                sut.Lock(theManager));
        };

        private Because of = () =>
            sut.Unlock(theManager);

        private It should_not_be_locked = () =>
            sut.IsLocked.ShouldBeFalse();
    }

    [Subject(typeof (Membership))]
    public class when_trying_to_unlock_own_membership : membership_concern
    {
        private static Manager theManager;

        private Establish ctx = () =>
        {
            theManager = make.Manager(theMemberId);
            sut_setup.run(sut =>
                sut.Lock(make.Manager()));
        };

        private Because of = () =>
            caughtException = Catch.Exception(() => sut.Unlock(theManager));

        private It should_throw_invalid_operation_exception = () =>
            caughtException.ShouldBeOfExactType<InvalidOperationException>();
    }
}