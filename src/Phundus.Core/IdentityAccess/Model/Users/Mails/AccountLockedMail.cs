namespace Phundus.IdentityAccess.Model.Users.Mails
{
    using Common.Eventing;
    using Common.Mailing;
    using IdentityAccess.Users.Model;
    using Resources;

    public class AccountLockedMail : MailBase,
        ISubscribeTo<UserLocked>
    {
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;
        private readonly IUserRepository _userRepository;

        public AccountLockedMail(IMessageFactory factory, IMailGateway gateway, IUserRepository userRepository)
        {
            _factory = factory;
            _gateway = gateway;
            _userRepository = userRepository;
        }

        public void Handle(UserLocked e)
        {
            var user = _userRepository.FindByGuid(e.UserId);
            var model = new Model
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress
            };

            var message = _factory.MakeMessage(model, Templates.AccountLockedSubject, null, Templates.AccountLockedHtml);
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