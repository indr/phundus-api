namespace Phundus.Core.IdentityAndAccessCtx.Mails
{
    using System;
    using DomainModel;
    using Infrastructure;
    using SettingsCtx;

    public class UserChangeEmailValidationMail : BaseMail
    {
        public UserChangeEmailValidationMail()
            : base(Settings.Mail.Templates.UserChangeEmailValidationMail)
        {
        }

        public void Send(User user)
        {
            Send(user.Membership.RequestedEmail);
        }

        public UserChangeEmailValidationMail For(User user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
                {
                    Settings = Settings.GetSettings(),
                    Urls = new Urls(Config.ServerUrl),
                    User = user,
                    Admins = Config.FeedbackRecipients
                };
            //DataContext.Add("User", user);
            //DataContext.Add("Membership", user.Membership);
            return this;
        }
    }
}