namespace Phundus.IdentityAccess.Model.Users.Mails
{
    using Common.Mailing;
    using Common.Notifications;
    using IdentityAccess.Users.Model;
    using Resources;

    public class EmailAddressValidationMail : IConsumes<UserEmailAddressChangeRequested>
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

        public void Handle(UserEmailAddressChangeRequested e)
        {
            var user = _userRepository.FindByGuid(e.UserGuid);
            var model = new Model
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ValidationKey = user.Account.ValidationKey
            };

            var message = _factory.MakeMessage(model, Templates.EmailAddressValidationSubject, null,
                Templates.EmailAddressValidationHtml);
            message.To.Add(user.Account.RequestedEmail);

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