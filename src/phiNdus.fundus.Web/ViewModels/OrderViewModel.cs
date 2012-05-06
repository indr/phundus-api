using System;
using System.ComponentModel.DataAnnotations;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Web.ViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel(OrderDto each)
        {
            Id = each.Id;
            CreateDate = each.CreateDate;

            ReserverName = each.ReserverName;
            ModifierName = each.ModifierName;
            Status = each.Status;

            ModifyDate = each.ModifyDate;

            TotalPrice = each.TotalPrice;
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
    }
}