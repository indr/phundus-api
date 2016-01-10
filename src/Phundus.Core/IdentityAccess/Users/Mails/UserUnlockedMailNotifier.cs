namespace Phundus.Core.IdentityAndAccess.Users.Mails
{
    using System;
    using Ddd;
    using Infrastructure;
    using Infrastructure.Gateways;
    using Model;
    using Repositories;

    public class UserUnlockedMailNotifier : BaseMail, ISubscribeTo<UserUnlocked>
    {
        private readonly IUserRepository _userRepository;

        public UserUnlockedMailNotifier(IMailGateway mailGateway, IUserRepository userRepository) : base(mailGateway)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            _userRepository = userRepository;
        }

        public void Handle(UserUnlocked @event)
        {
            var user = _userRepository.FindByGuid(@event.UserId);
            if (user == null)
                return;

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                User = user,
                Admins = Config.FeedbackRecipients
            };

            Send(user.Account.Email, Templates.UserUnlockedSubject, null, Templates.UserUnlockedHtml);
        }
    }
}