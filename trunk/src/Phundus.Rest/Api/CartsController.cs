namespace Phundus.Rest.Api
{
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Shop.Orders.Commands;

    [RoutePrefix("api/carts")]
    public class CartsController : ApiControllerBase
    {
        [DELETE("")]
        [Transaction]
        public virtual HttpResponseMessage Delete()
        {
            Dispatcher.Dispatch(new ClearCart {InitiatorId = CurrentUserId.Id});

            return NoContent();
        }
    }
}