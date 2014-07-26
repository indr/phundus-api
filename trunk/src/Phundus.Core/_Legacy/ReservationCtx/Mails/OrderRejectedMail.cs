namespace Phundus.Core.ReservationCtx.Mails
{
    using System;
    using System.Net.Mail;
    using IdentityAndAccess.Users.Model;
    using Infrastructure;
    using Model;
    using SettingsCtx;

    public class OrderRejectedMail : BaseMail
    {
        public OrderRejectedMail()
            : base(Settings.Mail.Templates.OrderRejected)
        {
        }

        public OrderRejectedMail For(Order order)
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

        public void Send(User user)
        {
            Send(user.Account.Email);
        }
    }
}