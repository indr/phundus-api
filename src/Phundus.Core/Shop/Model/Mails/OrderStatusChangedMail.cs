namespace Phundus.Shop.Orders.Mails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Mailing;
    using Model;
    using Services;
    using Shop.Model;
    using Shop.Model.Mails;

    public class OrderStatusChangedMail : OrderMailBase,
        ISubscribeTo<OrderApproved>,
        ISubscribeTo<OrderRejected>
    {
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;
        private readonly IOrderPdfGeneratorService _orderPdfGeneratorService;
        private readonly IOrderRepository _orderRepository;

        public OrderStatusChangedMail(IMessageFactory factory, IMailGateway gateway, IOrderRepository orderRepository,
            IOrderPdfGeneratorService orderPdfGeneratorService)
        {
            _factory = factory;
            _gateway = gateway;
            _orderRepository = orderRepository;
            _orderPdfGeneratorService = orderPdfGeneratorService;
        }

        public void Handle(OrderApproved e)
        {
            if (e.Lessee == null)
                return;

            var order = _orderRepository.GetById(new OrderId(e.OrderId));
            var stream = _orderPdfGeneratorService.GeneratePdf(order);
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

            var message = _factory.MakeMessage(model, Templates.OrderApprovedSubject, null, Templates.OrderApprovedHtml);
            message.To.Add(e.Lessee.EmailAddress);
            message.Attachments.Add(attachment);

            _gateway.Send(e.OccuredOnUtc, message);
        }

        public void Handle(OrderRejected e)
        {
            if (e.Lessee == null)
                return;

            var order = _orderRepository.GetById(new OrderId(e.OrderId));
            var stream = _orderPdfGeneratorService.GeneratePdf(order);
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

            var message = _factory.MakeMessage(model, Templates.OrderRejectedSubject, null, Templates.OrderRejectedHtml);
            message.To.Add(e.Lessee.EmailAddress);
            message.Attachments.Add(attachment);

            _gateway.Send(e.OccuredOnUtc, message);
        }
    }
}