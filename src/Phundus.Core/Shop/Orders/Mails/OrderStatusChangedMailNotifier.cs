namespace Phundus.Core.Shop.Orders.Mails
{
    using System;
    using System.Net.Mail;
    using Ddd;
    using Infrastructure;
    using Model;
    using Repositories;
    using Services;

    public class OrderStatusChangedMailNotifier : BaseMail, ISubscribeTo<OrderApproved>, ISubscribeTo<OrderRejected>
    {
        public IOrderRepository OrderRepository { get; set; }

        public IOrderPdfGeneratorService OrderPdfGeneratorService { get; set; }

        public void Handle(OrderApproved @event)
        {
            var order = OrderRepository.GetById(@event.OrderId);

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                order.Borrower,
                Order = order,
                Admins = Config.FeedbackRecipients
            };

            Attachments.Add(new Attachment(OrderPdfGeneratorService.GeneratePdf(order),
                String.Format("Bestellung-{0}.pdf", order.Id),
                "application/pdf"));

            Send(order.Borrower.EmailAddress, Templates.OrderApprovedSubject, null, Templates.OrderApprovedHtml);
        }

        public void Handle(OrderRejected @event)
        {
            var order = OrderRepository.GetById(@event.OrderId);

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                order.Borrower,
                Order = order,
                Admins = Config.FeedbackRecipients
            };

            Attachments.Add(new Attachment(OrderPdfGeneratorService.GeneratePdf(order),
                String.Format("Bestellung-{0}.pdf", order.Id),
                "application/pdf"));

            Send(order.Borrower.EmailAddress, Templates.OrderRejectedSubject, null, Templates.OrderRejectedHtml);
        }
    }
}