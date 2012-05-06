using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Web.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        public OrderViewModel(OrderDto dto)
        {
            Load(dto);
        }

        public OrderViewModel(int id)
        {
            var dto = IoC.Resolve<IOrderService>().GetOrder(SessionId, id);
            Load(dto);
        }

        private void Load(OrderDto dto)
        {
            Id = dto.Id;
            CreateDate = dto.CreateDate;

            ReserverName = dto.ReserverName;
            ModifierName = dto.ModifierName;
            Status = dto.Status;

            ModifyDate = dto.ModifyDate;

            TotalPrice = dto.TotalPrice;

            Items = dto.Items;
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
        public double TotalPrice { get; set; }


        public IList<OrderItemDto> Items { get; set; }
    }
}