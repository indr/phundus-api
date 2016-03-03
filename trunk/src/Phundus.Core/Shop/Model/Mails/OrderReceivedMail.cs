namespace Phundus.Shop.Orders.Mails
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using Common;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Mailing;
    using Model;
    using Services;
    using Shop.Model;
    using Shop.Model.Mails;

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

        public void Handle(OrderPlaced e)
        {
            var order = _orderRepository.GetById(new OrderId(e.OrderId));
            var managers = _lessorService.GetManagersForEmailNotification(new LessorId(e.LessorId));

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                Lessee = order.Lessee,
                Order = order,
                Admins = Config.FeedbackRecipients
            };

            var stream = _orderPdfGeneratorService.GeneratePdf(order);
            Attachments.Add(new Attachment(stream, String.Format("Bestellung-{0}.pdf", order.OrderShortId.Id), "application/pdf"));

            var toAddresses = String.Join(",", managers.Select(s => s.EmailAddress));

            Send(toAddresses, Templates.OrderReceivedSubject, null, Templates.OrderReceivedHtml);
        }
    }
}