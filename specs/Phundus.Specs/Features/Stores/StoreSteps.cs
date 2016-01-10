namespace Phundus.Specs.Features.Stores
{
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class StoreSteps : StepsBase
    {
        public StoreSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"open user store")]
        public void WhenOpenUserStore()
        {
            var user = Ctx.User;
            App.OpenUserStore(user, false);
        }

        [Then(@"get user with store")]
        public void ThenGetUserWithStore()
        {
            var user = Ctx.User;
            var response = App.GetUser(user.Id);
            Assert.That(response.Store, Is.Not.Null);
            Assert.That(response.Store.StoreId, Is.EqualTo(user.StoreId));
        }
    }
}