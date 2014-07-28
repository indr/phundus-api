namespace Phundus.Core.IdentityAndAccess.Users.Mails
{
    using System;
    using Infrastructure;
    using Model;

    public class UserAccountValidationMail : BaseMail
    {
        public void Send(User user)
        {
            Send(user.Account.Email, Templates.UserAccountValidationSubject, null, Templates.UserAccountValidationHtml);
        }

        public UserAccountValidationMail For(User user)
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
    }
}