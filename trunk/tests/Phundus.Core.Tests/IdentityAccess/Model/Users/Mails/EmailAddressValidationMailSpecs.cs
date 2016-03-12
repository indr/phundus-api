namespace Phundus.Tests.IdentityAccess.Model.Users.Mails
{
    using System;
    using System.Net.Mail;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Users.Mails;
    using Phundus.IdentityAccess.Users.Model;
    using Rhino.Mocks;

    [Subject(typeof (EmailAddressValidationMail))]
    public class when_handling_user_email_address_change_requested : mail_concern<EmailAddressValidationMail>
    {
        private static UserEmailAddressChangeRequested e = new UserEmailAddressChangeRequested(initiator, new UserId(),
            "John", "Galt", "requested@test.phundus.ch", "validationKey");

        private Because of = () =>
            sut.Handle(e);

        private It should_send_to_the_requested_email_address = () =>
            message.To.ShouldContain(p => p.Address == "requested@test.phundus.ch");

        private It should_send_via_gateway = () =>
            gateway.received(x => x.Send(Arg<DateTime>.Is.Equal(e.OccuredOnUtc), Arg<MailMessage>.Is.NotNull));
    }
}