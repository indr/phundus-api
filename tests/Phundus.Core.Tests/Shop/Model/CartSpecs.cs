﻿namespace Phundus.Tests.Shop.Model
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Model;
    using Owner = Phundus.Shop.Orders.Model.Owner;

    public class cart_concern
    {
        protected static Cart sut;
        protected static InitiatorId theInitiatorId = new InitiatorId();
        protected static UserId theUserGuid = new UserId(Guid.NewGuid());

        private Establish ctx = () =>
        {
            sut = CreateEmptyCart();
            AddCartItem();
            AddCartItem();
            AddCartItem();
        };

        protected static Cart CreateEmptyCart()
        {
            return new Cart(theInitiatorId, theUserGuid);
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

        private It should_be_empty = () => sut.IsEmpty.ShouldBeTrue();
        private It should_have_the_user_id = () => sut.UserGuid.ShouldEqual(theUserGuid.Id);
    }

    [Subject(typeof (Cart))]
    public class when_adding_an_item : cart_concern
    {
        private static CartItemId theCartItemId;

        private Because of =
            () => theCartItemId = AddCartItem(article: CreateArticle(articleId: 12345, name: "Football"));

        private It should_have_an_item_with_item_id = () => sut.Items.ShouldContain(c =>
            Equals(c.CartItemId, theCartItemId));

        private It should_not_be_empty = () => sut.IsEmpty.ShouldBeFalse();
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