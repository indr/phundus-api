namespace Phundus.IdentityAccess.Model.Users.Mails
{
    using Common.Eventing;
    using Common.Mailing;
    using IdentityAccess.Users.Model;
    using Resources;

    public class EmailAddressValidationMail : ISubscribeTo<EmailAddressChangeRequested>
    {
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;
        private readonly IUserRepository _userRepository;

        public EmailAddressValidationMail(IMessageFactory factory, IMailGateway gateway, IUserRepository userRepository)
        {
            _factory = factory;
            _gateway = gateway;
            _userRepository = userRepository;
        }

        public void Handle(EmailAddressChangeRequested e)
        {
            var model = new Model
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                ValidationKey = e.ValidationKey
            };

            var message = _factory.MakeMessage(model, Templates.EmailAddressValidationSubject, null,
                Templates.EmailAddressValidationHtml);
            message.To.Add(e.RequestedEmailAddress);

            _gateway.Send(e.OccuredOnUtc, message);
        }

        public class Model : MailModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ValidationKey { get; set; }
        }
    }
}