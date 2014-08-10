namespace phiNdus.fundus.Web.ViewModels
{
    using System.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;
    using Phundus.Core.ReservationCtx;
    using Phundus.Core.ReservationCtx.Model;
    using Phundus.Core.Shop.Orders;
    using Phundus.Core.Shop.Orders.Model;
    using Phundus.Core.Shop.Queries;

    public class OrdersViewModelBase : ViewModelBase
    {
        private IList<OrderViewModel> _items = new List<OrderViewModel>();

        public IList<OrderViewModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        protected IOrderService Service
        {
            get { return ServiceLocator.Current.GetInstance<IOrderService>(); }
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
        public MyOrdersViewModel()
        {
            Load(Service.GetMyOrders());
        }
    }
}