namespace Phundus.Specs.Features.Account
{
    using System;
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

        [Given(@"change password")]
        public void AngenommenChangePassword()
        {
            var newPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
            var user = Ctx.User;
            App.ChangePassword(user.Guid, user.Password, newPassword);
            user.Password = newPassword;
        }

        [When(@"reset password")]
        public void WhenResetPassword()
        {
            App.ResetPassword(Ctx.User.Username);
        }

        [When(@"log in")]
        public void WhenLogIn()
        {
            var user = Ctx.User;
            Ctx.LoggedIn = App.LogIn(user.Username, user.Password , false);
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