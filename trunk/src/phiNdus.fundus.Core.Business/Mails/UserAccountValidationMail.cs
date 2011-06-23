using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Settings;

namespace phiNdus.fundus.Core.Business.Mails
{
    public class UserAccountValidationMail : AbstractMail
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
            DataContext.Add("User", user);
            return this;
        }
    }
}