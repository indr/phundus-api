namespace Phundus.Core.IdentityAndAccess.Users.Mails
{
    using System;
    using Infrastructure;
    using Model;

    public class UserLockedOutMail : BaseMail
    {
        public UserLockedOutMail Send(User user)
        {
            Send(user.Account.Email, Templates.UserLockedSubject, null, Templates.UserLockedHtml);
            return this;
        }

        public UserLockedOutMail For(User user)
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
            base.Send(address, Templates.UserLockedSubject, null, Templates.UserLockedHtml);
        }
    }
}