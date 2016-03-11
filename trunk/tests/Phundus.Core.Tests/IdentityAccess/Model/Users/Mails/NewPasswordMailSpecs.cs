namespace Phundus.Tests.IdentityAccess.Model.Users.Mails
{
    using System;
    using System.Net.Mail;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Users;
    using Phundus.IdentityAccess.Model.Users.Mails;
    using Rhino.Mocks;

    [Subject(typeof(NewPasswordMail))]
    public class when_handling_password_resetted : mail_concern<NewPasswordMail>
    {
        private static PasswordResetted e = new PasswordResetted(new UserId(), "John", "Galt", "user@test.phundus.ch", "password");

        private Because of = () =>
            sut.Handle(e);

        private It should_send_via_gateway = () =>
            gateway.received(x => x.Send(Arg<DateTime>.Is.Equal(e.OccuredOnUtc), Arg<MailMessage>.Is.NotNull));
    }
}
