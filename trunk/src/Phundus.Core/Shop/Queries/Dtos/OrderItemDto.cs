namespace Phundus.Core.Shop.Queries
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public int OrderId { get; set; }
        public int ArticleId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime From { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime To { get; set; }
        
        public int Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public double UnitPrice { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public double LineTotal { get; set; }

        public string Text { get; set; }

        public bool Availability { get; set; }
    }
}