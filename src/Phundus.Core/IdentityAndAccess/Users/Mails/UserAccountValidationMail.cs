﻿namespace Phundus.Core.IdentityAndAccess.Users.Mails
{
    using System;
    using Infrastructure;
    using Model;
    using SettingsCtx;

    public class UserAccountValidationMail : BaseMail
    {
        public UserAccountValidationMail() : base(Settings.Mail.Templates.UserAccountValidation)
        {
        }

        public void Send(User user)
        {
            Send(user.Account.Email);
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