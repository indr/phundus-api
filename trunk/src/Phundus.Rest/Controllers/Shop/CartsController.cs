namespace Phundus.Rest.Controllers.Shop
{
    using Castle.Transactions;
    using Core.Shop.Orders.Commands;

    public class CartsController : ApiControllerBase
    {
        [Transaction]
        public virtual void Delete()
        {
            Dispatcher.Dispatch(new ClearCart {UserId = CurrentUserId});
        }
    }
}