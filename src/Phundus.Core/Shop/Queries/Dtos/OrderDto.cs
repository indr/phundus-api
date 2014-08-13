namespace Phundus.Core.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using Orders.Model;

    public class OrderDto
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int OrganizationId { get; set; }

        public DateTime CreateDate { get; set; }

        private IList<OrderItemDto> _items = new List<OrderItemDto>();
        private BorrowerDto _borrower = new BorrowerDto();


        public decimal TotalPrice { get; set; }

        public IList<OrderItemDto> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public BorrowerDto Borrower
        {
            get { return _borrower; }
            set { _borrower = value; }
        }

        public DateTime? ModifiedOn { get; set; }
        public int? ModifierId { get; set; }
        public string ModifierName { get; set; }

        public OrderStatus Status { get; set; }
        
    }

    public class BorrowerDto
    {
        public int BorrowerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MemberNumber { get; set; }
    }
}