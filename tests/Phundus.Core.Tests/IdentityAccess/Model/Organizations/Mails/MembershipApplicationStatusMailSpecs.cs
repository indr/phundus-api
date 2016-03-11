namespace Phundus.Tests.IdentityAccess.Model.Organizations.Mails
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Integration.IdentityAccess;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Organizations.Mails;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Projections;
    using Rhino.Mocks;

    public class membership_application_status_mail_concern<T> : mail_concern<T> where T : class
    {
        private Establish ctx = () =>
        {
            var make = new identityaccess_factory(fake);
            depends.on<IUsersQueries>().setup(x => x.GetById(Arg<Guid>.Is.Anything)).Return(make.UserData());
            depends.on<IOrganizationQueries>()
                .setup(x => x.GetById(Arg<Guid>.Is.Anything))
                .Return(make.OrganizationData());
        };
    }

    [Subject(typeof (MembershipApplicationStatusMail))]
    public class when_handling_membership_application_filed :
        membership_application_status_mail_concern<MembershipApplicationStatusMail>
    {
        private static MembershipApplicationFiled e = new MembershipApplicationFiled(new InitiatorId(),
            new OrganizationId(), new UserId());

        private Establish ctx = () =>
        {
            var managers = new List<Manager>
            {
                new Manager(new UserId(), "manager1@test.phundus.ch", "The Manager 1"),
                new Manager(new UserId(), "manager2@test.phundus.ch", "The Manager 2")
            };
            depends.on<IMembersWithRole>().setup(x => x.Manager(Arg<Guid>.Is.Anything, Arg<bool>.Is.Anything)).Return(managers);
        };

        private Because of = () =>
            sut.Handle(e);

        private It should_not_send_to_user = () =>
            message.To.ShouldNotContain(p => p.Address == "user@test.phundus.ch");

        private It should_send_via_gateway = () =>
            gateway.received(x => x.Send(Arg<DateTime>.Is.Equal(e.OccuredOnUtc), Arg<MailMessage>.Is.NotNull));

        private It should_sent_to_managers = () =>
            message.To.ShouldContain(
                p => p.Address == "manager1@test.phundus.ch" || p.Address == "manager2@test.phundus.ch");
    }

    [Subject(typeof (MembershipApplicationStatusMail))]
    public class when_handling_membership_application_approved :
        membership_application_status_mail_concern<MembershipApplicationStatusMail>
    {
        private static MembershipApplicationApproved e = new MembershipApplicationApproved(new InitiatorId(),
            new OrganizationId(), new UserId());

        private Because of = () =>
            sut.Handle(e);

        private It should_send_to_user = () =>
            message.To.ShouldContain(p => p.Address == "user@test.phundus.ch");

        private It should_send_via_gateway = () =>
            gateway.received(x => x.Send(Arg<DateTime>.Is.Equal(e.OccuredOnUtc), Arg<MailMessage>.Is.NotNull));
    }

    [Subject(typeof (MembershipApplicationStatusMail))]
    public class when_handling_membership_application_rejected :
        membership_application_status_mail_concern<MembershipApplicationStatusMail>
    {
        private static MembershipApplicationRejected e = new MembershipApplicationRejected(new InitiatorId(),
            new OrganizationId(), new UserId());

        private Because of = () =>
            sut.Handle(e);

        private It should_send_to_user = () =>
            message.To.ShouldContain(p => p.Address == "user@test.phundus.ch");

        private It should_send_via_gateway = () =>
            gateway.received(x => x.Send(Arg<DateTime>.Is.Equal(e.OccuredOnUtc), Arg<MailMessage>.Is.NotNull));
    }
}