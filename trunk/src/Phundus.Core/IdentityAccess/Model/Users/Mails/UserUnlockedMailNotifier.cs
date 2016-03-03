namespace Phundus.IdentityAccess.Users.Mails
{
    using System;
    using Common;
    using Common.Eventing;
    using Common.Mailing;
    using IdentityAccess.Model.Users;
    using IdentityAccess.Model.Users.Mails;    
    using Model;

    public class UserUnlockedMailNotifier : BaseMail, ISubscribeTo<UserUnlocked>
    {
        private readonly IUserRepository _userRepository;

        public UserUnlockedMailNotifier(IMailGateway mailGateway, IUserRepository userRepository) : base(mailGateway)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            _userRepository = userRepository;
        }

        public void Handle(UserUnlocked e)
        {
            var user = _userRepository.FindByGuid(e.UserId);
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