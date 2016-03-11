namespace Phundus.Shop.Model.Mails
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Mailing;

    public abstract class OrderMailBase
    {
        public class LesseeModel
        {
            public string FullName { get; set; }
        }

        public class Model : MailModel
        {
            public OrderModel Order { get; set; }
            public LesseeModel Lessee { get; set; }
        }

        public class OrderLineModel
        {
            public string Text { get; set; }
            public Period Period { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPricePerWeek { get; set; }
            public decimal LineTotal { get; set; }
        }

        public class OrderModel
        {
            public DateTime CreatedAtUtc { get; set; }
            public int OrderShortId { get; set; }
            public ICollection<OrderLineModel> Lines { get; set; }
            public decimal OrderTotal { get; set; }
        }
    }
}