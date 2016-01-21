namespace Phundus.Specs.Features.Shop
{
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class PlaceOrderSteps : AppStepsBase
    {
        private int _orderId;

        public PlaceOrderSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"I try to place an order for ""(.*)""")]
        public void WhenIPlaceOrder(string organizationAlias)
        {
            _orderId = App.PlaceOrder(Ctx.User, Ctx.Organizations[organizationAlias].OrganizationId, false);
        }

        [Then(@"I should get an order id")]
        public void ThenIShouldGetAnOrderId()
        {
            Assert.That(_orderId, Is.GreaterThan(0));
        }
    }
}