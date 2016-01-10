namespace Phundus.Specs.Features.Stores
{
    using TechTalk.SpecFlow;

    [Binding]
    public class StoreSteps
    {
        [When(@"open user store")]
        public void WhenOpenUserStore()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"get user with store")]
        public void ThenGetUserWithStore()
        {
            ScenarioContext.Current.Pending();
        }
    }
}