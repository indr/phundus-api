namespace Phundus.Tests.Shop.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Shop.Model;
    using Phundus.Shop.Model.Products;

    public class cart_concern : Observes<Cart>
    {
        protected static shop_factory make;

        protected static InitiatorId theInitiatorId;
        protected static UserId theUserId;

        private Establish ctx = () =>
        {
            theInitiatorId = new InitiatorId();
            theUserId = new UserId();

            make = new shop_factory(fake);

            sut_factory.create_using(() =>
                new Cart(theUserId));
        };

        protected static CartItemId AddCartItem(Article product = null, Period period = null, int? quantity = null)
        {
            return sut.AddItem(
                new CartItemId(),
                product ?? make.Product(),
                period ?? Period.FromNow(1),
                quantity ?? 1);
        }
    }

    [Subject(typeof (Cart))]
    public class when_creating_a_cart : cart_concern
    {
        private Because of = () =>
            sut = new Cart(theUserId);

        private It should_be_empty = () =>
            sut.IsEmpty.ShouldBeTrue();

        private It should_have_the_user_id = () =>
            sut.UserId.Id.ShouldEqual(theUserId.Id);
    }

    [Subject(typeof (Cart))]
    public class when_adding_an_item : cart_concern
    {
        private static CartItemId theCartItemId = new CartItemId();

        private Because of = () =>
            sut.AddItem(theCartItemId, make.Product(), Period.FromNow(1), 2);

        private It should_have_an_item_with_item_id = () =>
            sut.Items.ShouldContain(c =>
                Equals(c.CartItemId, theCartItemId));

        private It should_not_be_empty = () =>
            sut.IsEmpty.ShouldBeFalse();
    }

    [Subject(typeof (Cart))]
    public class when_removing_an_item : cart_concern
    {
        private static CartItemId theCartItemId;

        private Establish ctx = () =>
            theCartItemId = AddCartItem();

        private Because of = () =>
            sut.RemoveItem(theCartItemId);

        private It should_not_contain_the_removed_item = () =>
            sut.Items.ShouldNotContain(c => Equals(c.CartItemId, theCartItemId));
    }

    [Subject(typeof (Cart))]
    public class when_trying_to_remove_an_non_existing_cart_item : cart_concern
    {
        private Because of = () => spec.catch_exception(() =>
            sut.RemoveItem(new CartItemId()));

        private It should_throw_invalid_operation_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<InvalidOperationException>();
    }

    [Subject(typeof (Cart))]
    public class when_taking_items : cart_concern
    {
        private static LessorId theLessorId = new LessorId();
        private static ICollection<CartItem> result;

        private Establish ctx = () =>
            sut_setup.run(sut =>
            {
                sut.AddItem(make.Product(theLessorId), Period.FromNow(1), 1);
                sut.AddItem(make.Product(), Period.FromNow(2), 2);
                sut.AddItem(make.Product(theLessorId), Period.FromNow(3), 3);
            });

        private Because of = () =>
            result = sut.TakeItems(theLessorId);

        private It should_return_2_items = () =>
            result.Count.ShouldEqual(2);

        private It should_return_all_items_for_that_lessor = () =>
            result.ShouldEachConformTo(c => Equals(c.LessorId, theLessorId));

        private It should_remove_items_from_cart = () =>
            sut.Items.ShouldContainOnly(sut.Items.Where(p => !Equals(p.LessorId, theLessorId)));
    }
}