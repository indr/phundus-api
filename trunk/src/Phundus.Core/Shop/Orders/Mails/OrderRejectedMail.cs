namespace Phundus.Core.Shop.Orders.Mails
{
    using System;
    using System.Net.Mail;
    using IdentityAndAccess.Organizations.Model;
    using IdentityAndAccess.Users.Model;
    using Infrastructure;
    using Model;

    public class OrderRejectedMail : BaseMail
    {
        public OrderRejectedMail For(Order order, Organization organization)
        {
            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                User = order.Reserver,
                Order = order,
                Admins = Config.FeedbackRecipients
            };

            Attachments.Add(new Attachment(order.GeneratePdf(organization),
                String.Format("Order-{0}.pdf", order.Id),
                "application/pdf"));

            return this;
        }

        public void Send(User user)
        {
            Send(user.Account.Email, Templates.OrderReceivedSubject, null, Templates.OrderRejectedHtml);
        }
    }
}