namespace Phundus.Tests.IdentityAccess.Model.Users.Mails
{
    using System;
    using System.Net.Mail;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Users;
    using Phundus.IdentityAccess.Model.Users.Mails;
    using Phundus.IdentityAccess.Users.Model;
    using Rhino.Mocks;

    [Subject(typeof (AccountLockedMail))]
    public class when_handling_user_locked : identityaccess_mail_concern<AccountLockedMail>
    {
        private static UserLocked e;

        private Establish ctx = () =>
        {
            e = new UserLocked(theAdmin, new UserId(), DateTime.UtcNow);
            var user = make.User();
            var userRepository = depends.on<IUserRepository>();
            userRepository.setup(x => x.FindByGuid(Arg<Guid>.Is.Anything)).Return(user);
        };

        private Because of = () =>
            sut.Handle(e);

        private It should_send_via_gateway = () =>
            gateway.received(x => x.Send(Arg<DateTime>.Is.Equal(e.OccuredOnUtc), Arg<MailMessage>.Is.NotNull));
    }
}