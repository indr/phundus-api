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
    using IdentityAccess.Application;
    using Infrastructure;
    using Orders;    
    using Shop.Orders.Model;

    public class OrderPlacedMail : OrderMailBase,
        ISubscribeTo<OrderPlaced>
    {
        private readonly ICollaboratorService _collaboratorService;
        private readonly IOrganizationQueryService _organizationQueryService;
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;
        private readonly ILessorService _lessorService;
        private readonly IOrderPdfStore _orderPdfStore;
        private readonly IOrderRepository _orderRepository;

        public OrderPlacedMail(IMessageFactory factory, IMailGateway gateway, IOrderRepository orderRepository,
            ILessorService lessorService, IOrderPdfStore orderPdfStore, ICollaboratorService collaboratorService, IOrganizationQueryService organizationQueryService)
        {
            _factory = factory;
            _gateway = gateway;
            _orderRepository = orderRepository;
            _lessorService = lessorService;
            _orderPdfStore = orderPdfStore;
            _collaboratorService = collaboratorService;
            _organizationQueryService = organizationQueryService;
        }

        public void Handle(OrderPlaced e)
        {
            var order = _orderRepository.GetById(new OrderId(e.OrderId));
            var managers = _collaboratorService.Managers(new LessorId(e.LessorId), true);
            var emailAddresses = managers.Select(s => s.EmailAddress).ToList();

            // 24.11.2017: Looks like when you order from a users store, collaborator service returns
            // an empty list for managers and this causes this handler to fail cause
            // there is no recipient for the email message.
            if (emailAddresses.Count <= 0)
            {
                return;
            }

            var orderPdf = _orderPdfStore.Get(order.OrderId);
            var attachment = new Attachment(orderPdf.GetStreamCopy(), String.Format("Bestellung-{0}.pdf", order.OrderShortId.Id),
                "application/pdf");

            var text = @"Die Materialien sind im angegeben Zeitraum nun für dich reserviert. Das Sekretariat bestätigt deine Buchung vor dem Abholdatum nochmals manuell und sendet dir die abschliessende Bestellungsgenehmigung. Bitte melde dich anschliessend beim Sekretariat um einen Abholtermin zu vereinbaren.";

            // This is a context violation. This email template setting belongs to the shop context.
            var organization = _organizationQueryService.FindById(e.LessorId);
            if (organization != null && !String.IsNullOrWhiteSpace(organization.OrderReceivedText))
            {
                text = organization.OrderReceivedText;
            }

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
                },
                Text = text
            };

            var message = _factory.MakeMessage(model, Templates.OrderReceivedSubject, null, Templates.OrderReceivedHtml);
            message.To.Add(String.Join(",", emailAddresses));
            message.Attachments.Add(attachment);

            _gateway.Send(e.OccuredOnUtc, message);
        }
    }
}