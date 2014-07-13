﻿namespace phiNdus.fundus.Domain.Mails
{
    using System;
    using Entities;
    using Infrastructure;
    using Phundus.Infrastructure;
    using Settings;

    public class UserResetPasswordMail : BaseMail
    {
        public UserResetPasswordMail()
            : base(Settings.Mail.Templates.UserResetPasswordMail)
        {
        }

        public void Send(User user)
        {
            Send(user.Membership.Email);
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