namespace Phundus.Specs.Features.Shop
{
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class PlaceOrderSteps : StepsBase
    {
        private int _orderId;

        public PlaceOrderSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"I try to place an order for ""(.*)""")]
        public void WhenIPlaceOrder(string organizationAlias)
        {
            _orderId = App.PlaceOrder(Ctx.User, Ctx.Organizations[organizationAlias].Guid);
        }

        [Then(@"I should get an order id")]
        public void ThenIShouldGetAnOrderId()
        {
            ScenarioContext.Current.Pending();
        }
    }
}