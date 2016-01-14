namespace Phundus.IdentityAccess.Users.Mails
{
    using System;
    using Ddd;
    using IdentityAccess.Users.Repositories;
    using Infrastructure;
    using Infrastructure.Gateways;
    using Model;

    public class UserLockedMailNotifier : BaseMail, ISubscribeTo<UserLocked>
    {
        private readonly IUserRepository _userRepository;

        public UserLockedMailNotifier(IMailGateway mailGateway, IUserRepository userRepository) : base(mailGateway)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            _userRepository = userRepository;
        }

        public void Handle(UserLocked @event)
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

            Send(user.Account.Email, Templates.UserLockedSubject, null, Templates.UserLockedHtml);
        }
    }
}