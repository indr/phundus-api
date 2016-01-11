namespace Phundus.Core.Shop.Orders.Mails
{
    using System;
    using System.Net.Mail;
    using Ddd;
    using Infrastructure;
    using Infrastructure.Gateways;
    using Model;
    using Phundus.Shop.Orders.Mails;
    using Repositories;
    using Services;

    public class OrderStatusChangedMailNotifier : BaseMail, ISubscribeTo<OrderApproved>, ISubscribeTo<OrderRejected>
    {
        public OrderStatusChangedMailNotifier(IMailGateway mailGateway) : base(mailGateway)
        {
        }

        public IOrderRepository OrderRepository { get; set; }

        public IOrderPdfGeneratorService OrderPdfGeneratorService { get; set; }

        public void Handle(OrderApproved @event)
        {
            var order = OrderRepository.GetById(@event.OrderId);

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                Borrower = order.Lessee,
                Order = order,
                Admins = Config.FeedbackRecipients
            };

            Attachments.Add(new Attachment(OrderPdfGeneratorService.GeneratePdf(order),
                String.Format("Bestellung-{0}.pdf", order.Id),
                "application/pdf"));

            Send(order.Lessee.EmailAddress, Templates.OrderApprovedSubject, null, Templates.OrderApprovedHtml);
        }

        public void Handle(OrderRejected @event)
        {
            var order = OrderRepository.GetById(@event.OrderId);

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                Borrower = order.Lessee,
                Order = order,
                Admins = Config.FeedbackRecipients
            };

            Attachments.Add(new Attachment(OrderPdfGeneratorService.GeneratePdf(order),
                String.Format("Bestellung-{0}.pdf", order.Id),
                "application/pdf"));

            Send(order.Lessee.EmailAddress, Templates.OrderRejectedSubject, null, Templates.OrderRejectedHtml);
        }
    }
}