namespace Phundus.Shop.Model.Mails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using Collaborators;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Mailing;
    using Infrastructure;
    using Orders;    
    using Shop.Orders.Model;

    public class OrderPlacedMail : OrderMailBase,
        ISubscribeTo<OrderPlaced>
    {
        private readonly ICollaboratorService _collaboratorService;
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;
        private readonly ILessorService _lessorService;
        private readonly IOrderPdfFactory _orderPdfFactory;
        private readonly IOrderRepository _orderRepository;

        public OrderPlacedMail(IMessageFactory factory, IMailGateway gateway, IOrderRepository orderRepository,
            ILessorService lessorService, IOrderPdfFactory orderPdfFactory, ICollaboratorService collaboratorService)
        {
            _factory = factory;
            _gateway = gateway;
            _orderRepository = orderRepository;
            _lessorService = lessorService;
            _orderPdfFactory = orderPdfFactory;
            _collaboratorService = collaboratorService;
        }

        public void Handle(OrderPlaced e)
        {
            var order = _orderRepository.GetById(new OrderId(e.OrderId));
            var managers = _collaboratorService.Managers(new LessorId(e.LessorId), true);
            var emailAddresses = managers.Select(s => s.EmailAddress).ToList();
            var stream = _orderPdfFactory.GeneratePdf(order);
            var attachment = new Attachment(stream, String.Format("Bestellung-{0}.pdf", order.OrderShortId.Id),
                "application/pdf");

            var model = new Model
            {
                Lessee = new LesseeModel
                {
                    FullName = e.Lessee.FullName
                },
                Order = new OrderModel
                {
                    CreatedAtUtc = e.OccuredOnUtc,
                    OrderShortId = e.OrderShortId,
                    Lines = e.Items == null
                        ? new List<OrderLineModel>()
                        : e.Items.Select(l => new OrderLineModel
                        {
                            Text = l.Text,
                            Period = l.Period,
                            Quantity = l.Quantity,
                            UnitPricePerWeek = l.UnitPricePerWeek,
                            LineTotal = l.LineTotal
                        }).ToList(),
                    OrderTotal = e.OrderTotal
                }
            };

            var message = _factory.MakeMessage(model, Templates.OrderReceivedSubject, null, Templates.OrderReceivedHtml);
            message.To.Add(String.Join(",", emailAddresses));
            message.Attachments.Add(attachment);

            _gateway.Send(e.OccuredOnUtc, message);
        }
    }
}