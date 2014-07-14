namespace Phundus.Core.Mails
{
    using System;
    using System.Net.Mail;
    using Entities;
    using IdentityAndAccessCtx.DomainModel;
    using Infrastructure;

    public class OrderReceivedMail : BaseMail
    {
        public OrderReceivedMail()
            : base(Settings.Settings.Mail.Templates.OrderReceived)
        {
        }

        public OrderReceivedMail For(Order order)
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

        public OrderReceivedMail Send(User user)
        {
            Send(user.Membership.Email);
            return this;
        }

        public new OrderReceivedMail Send(string address)
        {
            base.Send(address);
            return this;
        }
    }
}