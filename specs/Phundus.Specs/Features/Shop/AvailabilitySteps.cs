namespace Phundus.Specs.Features.Shop
{
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class AvailabilitySteps : AppStepsBase
    {
        private bool _checkResult;

        public AvailabilitySteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"I try to check availability with a quantity of (.*)")]
        public void WhenITryToCheckAvailabilityWithAQuantityOf(int quantity)
        {
            _checkResult = App.CheckAvailability(Ctx.Article, quantity);
        }

        [When(@"I try to check availibility with a quantity of (.*)")]
        public void WhenITryToCheckAvailibilityWithAQuantityOf(int quantity)
        {
            _checkResult = App.CheckAvailability(Ctx.Article, quantity);
        }

        [Then(@"I should see available")]
        public void ThenIShouldSeeAvailable()
        {
            Assert.That(_checkResult, Is.True);
        }

        [Then(@"I should see not available")]
        public void ThenIShouldSeeNotAvailable()
        {
            Assert.That(_checkResult, Is.False);
        }
    }
}