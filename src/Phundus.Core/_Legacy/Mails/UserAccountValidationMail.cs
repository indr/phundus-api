namespace Phundus.Core.Mails
{
    using System;
    using Entities;
    using IdentityAndAccessCtx.DomainModel;
    using Infrastructure;
    using Phundus.Infrastructure;

    public class UserAccountValidationMail : BaseMail
    {
        public UserAccountValidationMail() : base(Settings.Settings.Mail.Templates.UserAccountValidation)
        {
        }

        public void Send(User user)
        {
            Send(user.Membership.Email);
        }

        public UserAccountValidationMail For(User user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
                {
                    Settings = Settings.Settings.GetSettings(),
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