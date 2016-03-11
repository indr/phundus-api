using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phundus.Tests.Shop.Model.Orders.Mails
{
    using System.IO;
    using System.Net.Mail;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Mails;
    using Phundus.Shop.Orders.Model;
    using Phundus.Shop.Orders.Services;
    using Rhino.Mocks;
    using Manager = Integration.IdentityAccess.Manager;

    public class order_status_changed_mail_concern : mail_concern<OrderStatusChangedMail>
    {
    }

    [Subject(typeof (OrderStatusChangedMail))]
    public class when_handling_order_approved : order_status_changed_mail_concern
    {
        private static OrderApproved e;

        private Establish ctx = () =>
        {
            var make = new shop_factory(fake);

            depends.on<IOrderRepository>().setup(x => x.GetById(Arg<OrderId>.Is.Anything)).Return(make.Order());
            depends.on<IOrderPdfGeneratorService>()
                .setup(x => x.GeneratePdf(Arg<Order>.Is.Anything))
                .Return(new MemoryStream());

            var lines = new List<OrderEventLine>
            {
                new OrderEventLine(new OrderLineId(), new ArticleId(), new ArticleShortId(99), "Text", 1.23m,
                    Period.FromNow(1), 1, 1.23m)
            };
            e = new OrderApproved(make.Manager(), new OrderId(), new OrderShortId(123),
                make.Lessor(), make.Lessee(), OrderStatus.Pending, 12.34m, lines);
        };

        private Because of = () =>
            sut.Handle(e);

        private It should_have_attachement = () =>
            message.Attachments.ShouldNotBeEmpty();

        private It should_send_to_managers = () =>
            message.To.ShouldContain(p => p.Address == "lessee@test.phundus.ch");

        private It should_send_via_gateway = () =>
            gateway.received(x => x.Send(Arg<DateTime>.Is.Equal(e.OccuredOnUtc), Arg<MailMessage>.Is.NotNull));
    }

    [Subject(typeof (OrderStatusChangedMail))]
    public class when_handling_order_rejcted : order_status_changed_mail_concern
    {
        private static OrderRejected e;

        private Establish ctx = () =>
        {
            var make = new shop_factory(fake);

            depends.on<IOrderRepository>().setup(x => x.GetById(Arg<OrderId>.Is.Anything)).Return(make.Order());
            depends.on<IOrderPdfGeneratorService>()
                .setup(x => x.GeneratePdf(Arg<Order>.Is.Anything))
                .Return(new MemoryStream());

            var lines = new List<OrderEventLine>
            {
                new OrderEventLine(new OrderLineId(), new ArticleId(), new ArticleShortId(99), "Text", 1.23m,
                    Period.FromNow(1), 1, 1.23m)
            };
            e = new OrderRejected(make.Manager(), new OrderId(), new OrderShortId(123),
                make.Lessor(), make.Lessee(), OrderStatus.Pending, 12.34m, lines);
        };

        private Because of = () =>
            sut.Handle(e);

        private It should_have_attachement = () =>
            message.Attachments.ShouldNotBeEmpty();

        private It should_send_to_managers = () =>
            message.To.ShouldContain(p => p.Address == "lessee@test.phundus.ch");

        private It should_send_via_gateway = () =>
            gateway.received(x => x.Send(Arg<DateTime>.Is.Equal(e.OccuredOnUtc), Arg<MailMessage>.Is.NotNull));
    }
}
