namespace Phundus.Core.ReservationCtx.Mails
{
    using System;
    using System.Net.Mail;
    using IdentityAndAccessCtx.DomainModel;
    using Infrastructure;
    using Model;
    using SettingsCtx;

    public class OrderReceivedMail : BaseMail
    {
        public OrderReceivedMail()
            : base(Settings.Mail.Templates.OrderReceived)
        {
        }

        public OrderReceivedMail For(Order order)
        {
            Model = new
                {
                    Settings = Settings.GetSettings(),
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
            Send(user.SiteMembership.Email);
            return this;
        }

        public new OrderReceivedMail Send(string address)
        {
            base.Send(address);
            return this;
        }
    }
}