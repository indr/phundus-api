namespace Phundus.Core.IdentityAndAccess.Users.Mails
{
    using System;
    using Infrastructure;
    using Model;

    public class UserUnlockedMail : BaseMail
    {
        public UserUnlockedMail Send(User user)
        {
            Send(user.Account.Email, Templates.UserUnlockedSubject, null, Templates.UserUnlockedHtml);
            return this;
        }

        public UserUnlockedMail For(User user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                User = user,
                Admins = Config.FeedbackRecipients
            };

            return this;
        }

        public void Send(string address)
        {
            base.Send(address, Templates.UserUnlockedSubject, null, Templates.UserUnlockedHtml);
        }
    }
}