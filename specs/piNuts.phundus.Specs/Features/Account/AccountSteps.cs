namespace Phundus.Specs.Features.Account
{
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class AccountSteps : StepsBase
    {
        public AccountSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"a user")]
        public void GivenAUser()
        {
            var user = App.SignUpUser();
            Ctx.User = user;
        }

        [When(@"reset password")]
        public void WhenResetPassword()
        {
            App.ResetPassword(Ctx.User.EmailAddress);
        }

        [When(@"log in")]
        public void WhenLogIn()
        {
            Ctx.LoggedIn = App.LogIn(Ctx.User.EmailAddress, "secret", false);
        }

        [Then(@"logged in")]
        public void ThenLoggedIn()
        {
            Assert.That(Ctx.LoggedIn, Is.EqualTo(Ctx.User.Guid));
        }

        [Then(@"not logged in")]
        public void ThenNotLoggedIn()
        {
            Assert.That(Ctx.LoggedIn, Is.Not.EqualTo(Ctx.User.Guid));
        }
    }
}