namespace phiNdus.fundus.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Phundus.Core.Shop.Orders.Model;
    using Phundus.Core.Shop.Queries;

    public class OrderViewModel : ViewModelBase
    {
        public OrderViewModel(OrderDto dto)
        {
            Load(dto);
        }

        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime CreateDate { get; set; }


        public OrderStatus Status { get; set; }

        public string ReserverName { get; set; }
        public int ItemCount { get; set; }

        public DateTime? ModifyDate { get; set; }
        public string ModifierName { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal TotalPrice { get; set; }


        public IList<OrderItemDto> Items { get; set; }

        private void Load(OrderDto dto)
        {
            Id = dto.Id;
            CreateDate = dto.CreateDate;

            ReserverName = dto.Borrower.FirstName + " " + dto.Borrower.LastName;
            ModifierName = dto.ModifierName;
            Status = dto.Status;

            ModifyDate = dto.ModifiedOn;

            TotalPrice = dto.TotalPrice;

            Items = dto.Items;
        }
    }
}