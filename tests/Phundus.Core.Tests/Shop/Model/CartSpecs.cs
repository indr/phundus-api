namespace Phundus.Core.Tests.Shop.Model
{
    using System;
    using Common.Domain.Model;
    using Core.Shop.Orders.Model;
    using Machine.Specifications;

    public class cart_concern
    {
        protected static Cart sut;
        protected static UserId theUserId = new UserId(1001);
        protected static UserGuid theUserGuid = new UserGuid(Guid.NewGuid());

        private Establish ctx = () =>
        {
            sut = CreateEmptyCart();
            AddCartItem();
            AddCartItem();
            AddCartItem();
        };

        protected static Cart CreateEmptyCart()
        {
            return new Cart(theUserId, theUserGuid);
        }

        protected static Article CreateArticle(int? articleId = null, Owner owner = null,
            string name = "", decimal? pricePerWeek = null)
        {
            return new Article(
                articleId ?? 10001,
                owner ?? new Owner(new OwnerId(), "Owner"),
                name ?? "Article",
                pricePerWeek ?? 7);
        }

        protected static CartItemId AddCartItem(Article article = null, DateTime? fromUtc = null,
            DateTime? toUtc = null, int? quantity = null)
        {
            return sut.AddItem(
                article ?? CreateArticle(),
                fromUtc ?? DateTime.UtcNow,
                toUtc ?? DateTime.UtcNow.AddDays(1),
                quantity ?? 1);
        }
    }

    [Subject(typeof (Cart))]
    public class when_creating_a_cart : cart_concern
    {
        private Because of = () => sut = CreateEmptyCart();

        private It should_have_the_user_id = () => sut.CustomerId.ShouldEqual(theUserId.Id);
    }

    [Subject(typeof (Cart))]
    public class when_adding_an_item : cart_concern
    {
        private static CartItemId theCartItemId;

        private Because of =
            () => theCartItemId = AddCartItem(article: CreateArticle(articleId: 12345, name: "Football"));

        private It should_have_an_item_with_item_id = () => sut.Items.ShouldContain(c =>
            Equals(c.CartItemId, theCartItemId));
    }

    [Subject(typeof (Cart))]
    public class when_removing_an_item : cart_concern
    {
        private static CartItemId theCartItemId;
        private Establish ctx = () => theCartItemId = AddCartItem();

        private Because of = () => sut.RemoveItem(theCartItemId);

        private It should_not_contain_the_removed_item =
            () => sut.Items.ShouldNotContain(c => Equals(c.CartItemId, theCartItemId));
    }
}