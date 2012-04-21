using System;
using System.Collections.Generic;

namespace phiNdus.fundus.Core.Business.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public DateTime CreateDate { get; set; }

        private IList<OrderItemDto> _items = new List<OrderItemDto>();


        public double TotalPrice { get; set; }

        public IList<OrderItemDto> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public int ReserverId { get; set; }
        public string ReserverName { get; set; }

        public DateTime? ModifyDate { get; set; }
        public int? ModifierId { get; set; }
        public string ModifierName { get; set; }
    }
}