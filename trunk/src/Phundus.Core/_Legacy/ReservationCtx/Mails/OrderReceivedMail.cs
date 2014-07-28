﻿namespace Phundus.Core.ReservationCtx.Mails
{
    using System;
    using System.Net.Mail;
    using IdentityAndAccess.Users.Model;
    using Infrastructure;
    using Model;
    using _Legacy.ReservationCtx.Mails;

    public class OrderReceivedMail : BaseMail
    {
        public OrderReceivedMail For(Order order)
        {
            Model = new
            {
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
            Send(user.Account.Email, Templates.OrderReceivedSubject, null, Templates.OrderReceivedHtml);
            return this;
        }

        public OrderReceivedMail Send(string address)
        {
            base.Send(address, Templates.OrderReceivedSubject, null, Templates.OrderReceivedHtml);
            return this;
        }
    }
}