namespace Phundus.Core.Shop.Orders.Mails
{
    using System;
    using System.IO;
    using System.Net.Mail;
    using IdentityAndAccess.Organizations.Model;
    using IdentityAndAccess.Users.Model;
    using Infrastructure;
    using Model;
    using Services;

    public class OrderReceivedMail : BaseMail
    {
        public OrderReceivedMail For(Stream pdf, Order order, Organization organization)
        {
            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                Borrower = order.Borrower,
                Order = order,
                Admins = Config.FeedbackRecipients
            };

            Attachments.Add(new Attachment(pdf, String.Format("Order-{0}.pdf", order.Id), "application/pdf"));

            return this;
        }

        public OrderReceivedMail Send(User user)
        {
            Send(user.Account.Email, Templates.OrderReceivedSubject, null, Templates.OrderReceivedHtml);
            return this;
        }

        public OrderReceivedMail Send(string emailAddress)
        {
            base.Send(emailAddress, Templates.OrderReceivedSubject, null, Templates.OrderReceivedHtml);
            return this;
        }
    }
}