using System;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel(OrderDto each)
        {
            Id = each.Id;
        }

        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string ReserverName { get; set; }
        public int ItemCount { get; set; }

        public DateTime ModifyDate { get; set; }
        public string ModifierName { get; set; }
    }
}