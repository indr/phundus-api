namespace Phundus.Tests.Shop.Model.Orders.Mails
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mail;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Shop.Infrastructure;
    using Phundus.Shop.Model;
    using Phundus.Shop.Model.Collaborators;
    using Phundus.Shop.Model.Mails;
    using Phundus.Shop.Model.Orders;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    [Subject(typeof (OrderPlacedMail))]
    public class when_handling_order_recieved : mail_concern<OrderPlacedMail>
    {
        private static OrderPlaced e;

        private Establish ctx = () =>
        {
            var make = new shop_factory(fake);

            depends.on<IOrderRepository>().setup(x => x.GetById(Arg<OrderId>.Is.Anything)).Return(make.Order());
            depends.on<ICollaboratorService>()
                .setup(x => x.Managers(Arg<LessorId>.Is.Anything, Arg<bool>.Is.Equal(true)))
                .Return(new List<Manager>
                {
                    make.Manager(emailAddress: "manager1@test.phundus.ch"),
                    make.Manager(emailAddress: "manager2@test.phundus.ch")
                });
            depends.on<IOrderPdfStore>()
                .setup(x => x.Get(Arg<OrderId>.Is.Anything, Arg<int>.Is.Anything))
                .Return(new MemoryStream());            

            var lines = new List<OrderEventLine>
            {
                new OrderEventLine(new OrderLineId(), new ArticleId(), new ArticleShortId(99),
                    new StoreId(), "Text", 1.23m, Period.FromNow(1), 1, 1.23m)
            };
            e = new OrderPlaced(initiator, new OrderId(), new OrderShortId(123),
                make.Lessor(), make.Lessee(), OrderStatus.Pending, 12.34m, lines);
        };

        private Because of = () =>
            sut.Handle(e);

        private It should_have_attachement = () =>
            message.Attachments.ShouldNotBeEmpty();

        private It should_send_to_managers = () =>
            message.To.ShouldContain(
                p => p.Address == "manager1@test.phundus.ch" || p.Address == "manager2@test.phundus.ch");

        private It should_send_via_gateway = () =>
            gateway.received(x => x.Send(Arg<DateTime>.Is.Equal(e.OccuredOnUtc), Arg<MailMessage>.Is.NotNull));
    }
}