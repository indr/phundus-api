namespace Phundus.Specs.Features.Cart
{
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class CartSteps : StepsBase
    {
        private UsersCartGetOkResponseContent _cart;

        public CartSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"I view my cart")]
        public void WhenIViewMyCart()
        {
            _cart = App.GetCart(Ctx.User);
        }

        [Then(@"my cart should be empty")]
        public void ThenMyCartShouldBeEmpty()
        {
            Assert.That(_cart.Items, Has.Count.EqualTo(0));
        }

    }
}