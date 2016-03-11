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

    [Subject(typeof (EmailAddressValidationMail))]
    public class when_handling_user_email_address_change_requested : mail_concern<EmailAddressValidationMail>
    {
        private static UserEmailAddressChangeRequested e = new UserEmailAddressChangeRequested(Guid.NewGuid(), new UserId());

        private Establish ctx = () =>
        {
            var user = new identityaccess_factory(fake).User();
            var userRepository = depends.on<IUserRepository>();
            userRepository.setup(x => x.FindByGuid(Arg<Guid>.Is.Anything)).Return(user);
        };

        private Because of = () =>
            sut.Handle(e);

        private It should_send_via_gateway = () =>
            gateway.received(x => x.Send(Arg<DateTime>.Is.Equal(e.OccuredOnUtc), Arg<MailMessage>.Is.NotNull));
    }
}