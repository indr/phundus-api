namespace Phundus.Shop.Orders.Mails
{
    using System;
    using System.Net.Mail;
    using Common;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Mailing;
    using Model;
    using Services;
    using Shop.Model;
    using Shop.Model.Mails;

    public class OrderStatusChangedMailNotifier : BaseMail,
        ISubscribeTo<OrderApproved>,
        ISubscribeTo<OrderRejected>
    {
        public OrderStatusChangedMailNotifier(IMailGateway mailGateway) : base(mailGateway)
        {
        }

        public IOrderRepository OrderRepository { get; set; }

        public IOrderPdfGeneratorService OrderPdfGeneratorService { get; set; }

        public void Handle(OrderApproved e)
        {
            var order = OrderRepository.GetById(new OrderId(e.OrderId));

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                order.Lessee,
                Order = order,
                Admins = Config.FeedbackRecipients
            };

            Attachments.Add(new Attachment(OrderPdfGeneratorService.GeneratePdf(order),
                String.Format("Bestellung-{0}.pdf", order.OrderShortId.Id),
                "application/pdf"));

            Send(e.OccuredOnUtc, order.Lessee.EmailAddress, Templates.OrderApprovedSubject, null,
                Templates.OrderApprovedHtml);
        }

        public void Handle(OrderRejected e)
        {
            var order = OrderRepository.GetById(new OrderId(e.OrderId));

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                Borrower = order.Lessee,
                Order = order,
                Admins = Config.FeedbackRecipients
            };

            Attachments.Add(new Attachment(OrderPdfGeneratorService.GeneratePdf(order),
                String.Format("Bestellung-{0}.pdf", order.OrderShortId.Id),
                "application/pdf"));

            Send(e.OccuredOnUtc, order.Lessee.EmailAddress, Templates.OrderRejectedSubject, null,
                Templates.OrderRejectedHtml);
        }
    }
}