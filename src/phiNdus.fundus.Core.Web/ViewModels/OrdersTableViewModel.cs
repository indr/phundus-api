using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class OrdersTableViewModel
    {
        public IEnumerable<OrderDto> Orders { get; set; }
    }
}