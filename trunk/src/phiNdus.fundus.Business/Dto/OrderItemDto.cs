using System;

namespace phiNdus.fundus.Core.Business.Dto
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public int OrderId { get; set; }
        public int ArticleId { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Amount { get; set; }

        public double UnitPrice { get; set; }
        public double LineTotal { get; set; }

        public string Text { get; internal set; }
    }
}