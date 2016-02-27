namespace Phundus.Shop.Orders.Mails
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using Common.Domain.Model;
    using Ddd;
    using Infrastructure;
    using Infrastructure.Gateways;
    using Model;
    using Repositories;
    using Services;
    using Shop.Model.Mails;
    using Shop.Services;

    public class OrderReceivedMail : BaseMail, ISubscribeTo<OrderPlaced>
    {
        private readonly ILessorService _lessorService;
        private readonly IOrderPdfGeneratorService _orderPdfGeneratorService;
        private readonly IOrderRepository _orderRepository;

        public OrderReceivedMail(IMailGateway mailGateway, IOrderRepository orderRepository,
            ILessorService lessorService, IOrderPdfGeneratorService orderPdfGeneratorService) : base(mailGateway)
        {
            if (orderRepository == null) throw new ArgumentNullException("orderRepository");
            if (lessorService == null) throw new ArgumentNullException("lessorService");
            if (orderPdfGeneratorService == null) throw new ArgumentNullException("orderPdfGeneratorService");
            _orderRepository = orderRepository;
            _lessorService = lessorService;
            _orderPdfGeneratorService = orderPdfGeneratorService;
        }

        public void Handle(OrderPlaced @event)
        {
            var order = _orderRepository.GetById(@event.OrderShortId);
            var managers = _lessorService.GetManagersForEmailNotification(new LessorId(@event.LessorId));

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                Lessee = order.Lessee,
                Order = order,
                Admins = Config.FeedbackRecipients
            };

            var stream = _orderPdfGeneratorService.GeneratePdf(order);
            Attachments.Add(new Attachment(stream, String.Format("Order-{0}.pdf", order.OrderShortId.Id), "application/pdf"));

            var toAddresses = String.Join(",", managers.Select(s => s.EmailAddress));

            Send(toAddresses, Templates.OrderReceivedSubject, null, Templates.OrderReceivedHtml);
        }
    }
}