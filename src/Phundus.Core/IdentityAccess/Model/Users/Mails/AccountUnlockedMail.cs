namespace Phundus.IdentityAccess.Model.Users.Mails
{
    using Common.Mailing;
    using Common.Notifications;
    using IdentityAccess.Users.Model;

    public class AccountUnlockedMail : IConsumes<UserUnlocked>
    {
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;
        private readonly IUserRepository _userRepository;

        public AccountUnlockedMail(IMessageFactory factory, IMailGateway gateway, IUserRepository userRepository)
        {
            _factory = factory;
            _gateway = gateway;
            _userRepository = userRepository;
        }

        public void Handle(UserUnlocked e)
        {
            var user = _userRepository.FindByGuid(e.UserId);
            var model = new Model
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress
            };

            var message = _factory.MakeMessage(model, Resources.Templates.AccountUnlockedSubject, null,
                Resources.Templates.AccountUnlockedHtml);
            message.To.Add(user.EmailAddress);

            _gateway.Send(e.OccuredOnUtc, message);
        }

        public class Model : MailModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EmailAddress { get; set; }
        }
    }
}