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

    [Subject(typeof(AccountValidationMail))]
    public class when_handling_user_signed_up : mail_concern<AccountValidationMail>
    {
        private static readonly UserSignedUp e = new UserSignedUp(new UserId(), new UserShortId(1),
            "user@test.phundus.ch", "password", "salt", "key", "John", "Galt", "Street", "Postoce", "City", "Phone number");

        private Because of = () =>
            sut.Handle(e);

        private It should_send_via_gateway = () =>
            gateway.received(x => x.Send(Arg<DateTime>.Is.Equal(e.OccuredOnUtc), Arg<MailMessage>.Is.NotNull));
    }
}