using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.ViewModels
{
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
            get { return IoC.Resolve<IOrderService>(); }
        }
    }

    public class OrdersViewModel : OrdersViewModelBase
    {
        
    }

    public class MyOrdersViewModel : OrdersViewModelBase
    {
        public MyOrdersViewModel() : base()
        {
            Load(Service.GetOrders(SessionId));
        }
    }
}