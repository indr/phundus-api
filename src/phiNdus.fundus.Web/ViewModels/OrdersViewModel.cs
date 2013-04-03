using System.Collections.Generic;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Web.ViewModels
{
    using phiNdus.fundus.Domain;
    using piNuts.phundus.Infrastructure;

    public class OrdersViewModelBase: ViewModelBase
    {
        private IList<OrderViewModel> _items = new List<OrderViewModel>();

        public IList<OrderViewModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        protected void Load(IList<OrderDto> orderDtos)
        {
            foreach (var each in orderDtos)
                Items.Add(new OrderViewModel(each));
        }

        protected IOrderService Service
        {
            get { return GlobalContainer.Resolve<IOrderService>(); }
        }
    }

    public class OrdersViewModel : OrdersViewModelBase
    {
        public OrdersViewModel(OrderStatus status)
        {
            Load(Service.GetOrders(SessionId, status));
        }
    }

    public class MyOrdersViewModel : OrdersViewModelBase
    {
        public MyOrdersViewModel() : base()
        {
            Load(Service.GetMyOrders(SessionId));
        }
    }
}