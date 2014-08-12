namespace Phundus.Core.Shop.Orders.Mails
{
    using System;
    using System.Net.Mail;
    using IdentityAndAccess.Organizations.Model;
    using IdentityAndAccess.Users.Model;
    using Infrastructure;
    using Model;
    using Services;

    public class OrderRejectedMail : BaseMail
    {
        public OrderRejectedMail For(Order order, Organization organization)
        {
            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                Borrower = order.Borrower,
                Order = order,
                Admins = Config.FeedbackRecipients
            };

            Attachments.Add(new Attachment(PdfGenerator.GeneratePdf(order, organization),
                String.Format("Order-{0}.pdf", order.Id),
                "application/pdf"));

            return this;
        }

        public void Send(string emailAddress)
        {
            Send(emailAddress, Templates.OrderReceivedSubject, null, Templates.OrderRejectedHtml);
        }
    }
}