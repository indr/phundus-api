namespace phiNdus.fundus.Domain.Mails
{
    using System;
    using System.Net.Mail;
    using Entities;
    using Infrastructure;
    using Settings;

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
                    Order = order
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