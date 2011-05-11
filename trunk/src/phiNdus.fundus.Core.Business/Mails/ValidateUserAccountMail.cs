using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Core.Domain;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Mails
{
    public class ValidateUserAccountMail : AbstractMail
    {
        public void Send(User user)
        {
            DataContext.Add("User", user);
            Send(user.Membership.Email);
        }

        protected override string Subject
        {
            get { return "[fundus] User Account Validation"; }
        }

        protected override string Body
        {
            get
            {
                return "Hello [User.FirstName]\r\n\r\nPlease go to the following link in order to validate your account:\r\n[Link.ValidateUserAccount]\r\n\r\nThanks";
            }
        }
    }
}
