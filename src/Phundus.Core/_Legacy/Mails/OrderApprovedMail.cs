namespace Phundus.Core.Mails
{
    using System;
    using System.Net.Mail;
    using Entities;
    using Infrastructure;

    public class OrderApprovedMail : BaseMail
    {
        public OrderApprovedMail()
            : base(Settings.Settings.Mail.Templates.OrderApproved)
        {
        }

        public OrderApprovedMail For(Order order)
        {
            Model = new
                {
                    Settings = Settings.Settings.GetSettings(),
                    Urls = new Urls(Config.ServerUrl),
                    User = order.Reserver,
                    Order = order,
                    Admins = Config.FeedbackRecipients
                };

            Attachments.Add(new Attachment(order.GeneratePdf(),
                                           String.Format("Order-{0}.pdf", order.Id),
                                           "application/pdf"));

            return this;
        }

        public void Send(User user)
        {
            Send(user.Membership.Email);
        }
    }
}