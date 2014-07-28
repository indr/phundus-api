namespace Phundus.Core.IdentityAndAccess.Users.Mails
{
    using System;
    using Infrastructure;
    using Model;
    using SettingsCtx;

    public class UserResetPasswordMail : BaseMail
    {
        public void Send(User user)
        {
            Send(user.Account.Email, Templates.UserResetPasswordSubject, null, Templates.UserResetPasswordHtml);
        }

        public UserResetPasswordMail For(User user, string password)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
                {
                    Settings = Settings.GetSettings(),
                    Urls = new Urls(Config.ServerUrl),
                    Password = password,
                    User = user,
                    Admins = Config.FeedbackRecipients
                };
            return this;
        }
    }
}