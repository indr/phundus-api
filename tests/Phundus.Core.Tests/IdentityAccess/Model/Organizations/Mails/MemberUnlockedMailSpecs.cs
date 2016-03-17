namespace Phundus.Tests.IdentityAccess.Model.Organizations.Mails
{
    using System;
    using System.Net.Mail;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Model.Organizations.Mails;
    using Phundus.IdentityAccess.Organizations.Model;
    using Rhino.Mocks;

    [Subject(typeof (MemberUnlockedMail))]
    public class when_handling_member_unlocked : mail_concern<MemberUnlockedMail>
    {
        private static MemberUnlocked e = new MemberUnlocked(new OrganizationId(), Guid.NewGuid());

        private Establish ctx = () =>
        {
            var make = new identityaccess_factory(fake);
            var userQueries = depends.on<IUsersQueries>();
            userQueries.setup(x => x.FindById(Arg<Guid>.Is.Anything)).Return(make.UserData());
        };

        private Because of = () =>
            sut.Handle(e);

        private It should_send_via_gateway = () =>
            gateway.received(x => x.Send(Arg<DateTime>.Is.Equal(e.OccuredOnUtc), Arg<MailMessage>.Is.NotNull));
    }
}