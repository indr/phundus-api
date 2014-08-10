namespace phiNdus.fundus.Web.ViewModels
{
    using System.Collections.Generic;
    using Phundus.Core.Shop.Queries;

    public class OrdersViewModelBase : ViewModelBase
    {
        private IList<OrderViewModel> _items = new List<OrderViewModel>();

        public IList<OrderViewModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public void Load(IEnumerable<OrderDto> orderDtos)
        {
            foreach (var each in orderDtos)
                Items.Add(new OrderViewModel(each));
        }
    }

    public class OrdersViewModel : OrdersViewModelBase
    {
        public OrdersViewModel(IEnumerable<OrderDto> orders)
        {
            Load(orders);
        }
    }

    public class MyOrdersViewModel : OrdersViewModelBase
    {
        public MyOrdersViewModel(IEnumerable<OrderDto> orders)
        {
            Load(orders);
        }
    }
}