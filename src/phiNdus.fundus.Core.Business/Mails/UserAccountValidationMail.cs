using System;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Settings;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Mails
{
    public class UserAccountValidationMail : BaseMail
    {
        public UserAccountValidationMail() : base(Settings.Mail.Templates.UserAccountValidation)
        {
        }

        public void Send(User user)
        {
            Send(user.Membership.Email);
        }

        public UserAccountValidationMail For(User user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            DataContext.Add("User", user);
            DataContext.Add("Membership", user.Membership);
            return this;
        }
    }
}