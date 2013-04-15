namespace phiNdus.fundus.Web.ViewModels
{
    using System.Collections.Generic;
    using Business.Dto;
    using Business.Services;
    using Domain.Entities;
    using Microsoft.Practices.ServiceLocation;

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

        protected void Load(IList<OrderDto> orderDtos)
        {
            foreach (var each in orderDtos)
                Items.Add(new OrderViewModel(each));
        }
    }

    public class OrdersViewModel : OrdersViewModelBase
    {
        public OrdersViewModel(OrderStatus status)
        {
            Load(Service.GetOrders(status));
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