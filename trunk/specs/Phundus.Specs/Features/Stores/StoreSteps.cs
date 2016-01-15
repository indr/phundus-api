namespace Phundus.Specs.Features.Stores
{
    using System.Net;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class StoreSteps : StepsBase
    {
        private UsersGetOkResponseContent _userDetails;

        public StoreSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"I opened my store")]
        public void GivenIOpenedMyStore()
        {
            App.OpenUserStore(Ctx.User);
            Ctx.Store = Ctx.User.StoreId;
        }

        [When(@"I try to open my store")]
        public void WhenITryToOpenMyStore()
        {
            var user = Ctx.User;
            App.OpenUserStore(user, false);
            Ctx.Store = Ctx.User.StoreId;
        }

        [When(@"I try to get my user details")]
        public void WhenITryToGetMyUserDetails()
        {
            var user = Ctx.User;
            _userDetails = App.GetUser(user.UserId);
        }

        [Then(@"I should see the store")]
        public void ThenIShouldSeeTheStore()
        {
            var user = Ctx.User;
            Assert.That(_userDetails.Store, Is.Not.Null);
            Assert.That(_userDetails.Store.StoreId, Is.EqualTo(user.StoreId));
        }
    }
}