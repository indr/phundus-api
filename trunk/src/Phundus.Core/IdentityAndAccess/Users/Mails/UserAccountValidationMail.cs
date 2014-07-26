namespace Phundus.Core.IdentityAndAccessCtx.Mails
{
    using System;
    using DomainModel;
    using Infrastructure;
    using SettingsCtx;

    public class UserAccountValidationMail : BaseMail
    {
        public UserAccountValidationMail() : base(Settings.Mail.Templates.UserAccountValidation)
        {
        }

        public void Send(User user)
        {
            Send(user.SiteMembership.Email);
        }

        public UserAccountValidationMail For(User user)
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