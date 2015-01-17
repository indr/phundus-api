using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phundus.Core.Tests.Shop.Domain.Model.Ordering
{
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using Orders.Model;
    using Rhino.Mocks;

    [Subject(typeof (Order))]
    public class when_order_is_approved : order_concern
    {
        public Because of = () => sut.Approve(_initiatorId.Id);

        public It should_publish_order_approved =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderApproved>.Is.NotNull));
    }
}
