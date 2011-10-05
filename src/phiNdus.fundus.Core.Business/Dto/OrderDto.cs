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


        public IList<OrderItemDto> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public int ReserverId { get; set; }
        public string ReserverName { get; set; }

        public DateTime? ApproveDate { get; set; }
        public int? ApproverId { get; set; }
        public string ApproverName { get; set; }

        public DateTime? RejectDate { get; set; }
        public int? RejecterId { get; set; }
        public string RejecterName { get; set; }
    }
}