namespace Phundus.Specs.Features.Cart
{
    using System;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class CartSteps : StepsBase
    {
        private UsersCartGetOkResponseContent _cart;
        private Guid _cartItemId;

        public CartSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"I added an article to cart")]
        public void GivenIAddedAnArticleToCart()
        {
            _cartItemId = App.AddArticleToCart(Ctx.User, Ctx.Article);
        }

        [When(@"I view my cart")]
        public void WhenIViewMyCart()
        {
            _cart = App.GetCart(Ctx.User);
        }

        [When(@"I try to add article to cart")]
        public void WhenIAddArticleToCart()
        {
            _cartItemId = App.AddArticleToCart(Ctx.User, Ctx.Article);
        }

        [When(@"I try remove the last cart item")]
        public void WhenITryRemoveTheLastCartItem()
        {
            App.RemoveCartItem(Ctx.User, _cartItemId);
        }

        [Then(@"my cart should be empty")]
        public void ThenMyCartShouldBeEmpty()
        {
            Assert.That(_cart.Items, Has.Count.EqualTo(0));
        }

        

    }
}