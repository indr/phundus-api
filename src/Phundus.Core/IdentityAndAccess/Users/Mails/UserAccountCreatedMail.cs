namespace Phundus.Core.IdentityAndAccess.Users.Mails
{
    using System;
    using Infrastructure;
    using Model;

    public class UserAccountCreatedMail : BaseMail
    {
        public void Send(User user)
        {
            Send(user.Account.Email, Templates.UserAccountCreatedSubject, null, Templates.UserAccountCreatedHtml);
        }

        public UserAccountCreatedMail For(User user)
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
            base.Send(address);
        }
    }
}