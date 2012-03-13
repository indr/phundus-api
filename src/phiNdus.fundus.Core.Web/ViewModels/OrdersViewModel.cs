using System.Collections.Generic;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class OrdersViewModel
    {
        private IList<OrderViewModel> _items = new List<OrderViewModel>();

        public IList<OrderViewModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }

    public class MyOrdersViewModel : OrdersViewModel
    {
        
    }
}