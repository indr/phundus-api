namespace Phundus.Core.IdentityAndAccess.Users.Mails
{
    using System;
    using Ddd;
    using Infrastructure;
    using Model;
    using Repositories;

    public class UserLockedMailNotifier : BaseMail, ISubscribeTo<UserLocked>
    {
        private readonly IUserRepository _userRepository;

        public UserLockedMailNotifier(IUserRepository userRepository)
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