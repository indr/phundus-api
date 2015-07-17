namespace Phundus.Rest.Api
{
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Shop.Orders.Commands;

    [RoutePrefix("api/carts")]
    public class CartsController : ApiControllerBase
    {
        [DELETE("")]
        [Transaction]
        public virtual void Delete()
        {
            Dispatcher.Dispatch(new ClearCart {InitiatorId = CurrentUserId});
        }
    }
}