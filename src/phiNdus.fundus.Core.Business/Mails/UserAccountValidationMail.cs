using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.Mails
{
    public class UserAccountValidationMail : AbstractMail
    {
        protected override string Subject
        {
            get { return "[fundus] User Account Validation"; }
        }

        protected override string Body
        {
            get
            {
                return
                    "Hello [User.FirstName]\r\n\r\nPlease go to the following link in order to validate your account:\r\n[Link.UserAccountValidation]\r\n\r\nThanks";
            }
        }

        public void Send(User user)
        {
            DataContext.Add("User", user);
            Send(user.Membership.Email);
        }
    }
}