﻿namespace Phundus.Core.IdentityAndAccess.Users.Mails
{
    using System;
    using Infrastructure;
    using Model;

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
                Urls = new Urls(Config.ServerUrl),
                Password = password,
                User = user,
                Admins = Config.FeedbackRecipients
            };

            return this;
        }
    }
}