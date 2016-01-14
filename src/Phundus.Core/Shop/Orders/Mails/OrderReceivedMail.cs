namespace Phundus.Shop.Orders.Mails
{
    using System;
    using System.IO;
    using System.Net.Mail;
    using IdentityAccess.Users.Model;
    using Infrastructure;
    using Infrastructure.Gateways;
    using Model;

    public class OrderReceivedMail : BaseMail
    {
        public OrderReceivedMail(IMailGateway mailGateway) : base(mailGateway)
        {
        }

        public OrderReceivedMail For(Stream pdf, Order order)
        {
            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                Borrower = order.Lessee,
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