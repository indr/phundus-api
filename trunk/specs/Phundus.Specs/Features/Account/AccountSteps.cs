namespace Phundus.Specs.Features.Account
{
    using System;
    using NUnit.Framework;
    using Services;
    using Services.Entities;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class AccountSteps : StepsBase
    {
        private bool _validateKeyResult;
        private bool _changedEmailAddressResult;

        public AccountSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"a user")]
        public void GivenAUser()
        {
            var user = App.SignUpUser();
            Ctx.User = user;
        }

        [When(@"sign up")]
        public void WhenSignUp()
        {
            var user = App.SignUpUser();
            Ctx.User = user;
        }

        [Given(@"change password")]
        public void GivenChangePassword()
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

        
        

        [Given(@"user changed email address")]
        public void GivenUserChangedEmailAddress()
        {
            ChangeEmailAddress(Ctx.User);
        }

        [Given(@"""(.*)"" changed email address to ""(.*)""")]
        public void GivenUserChangedEmailAddress(string userKey, string emailKey)
        {
            var user = Ctx.Users[userKey];
            if (!Ctx.Emails.ContainsKey(emailKey))
                Ctx.Emails[emailKey] = Guid.NewGuid().ToString("N").Substring(0, 8) + "@test.phundus.ch";
            var emailAddress = Ctx.Emails[emailKey];
            ChangeEmailAddress(user, emailAddress);
        }

        [When(@"change email address")]
        public void WhenChangeEmailAddress()
        {
            ChangeEmailAddress(Ctx.User);
        }

        [When(@"""(.*)"" changes email address to ""(.*)""")]
        public void WhenChangesEmailAddressTo(string userKey, string emailKey)
        {
            var user = Ctx.Users[userKey];
            var emailAddress = Ctx.Emails[emailKey];

            ChangeEmailAddress(user, emailAddress, false);
        }

        private void ChangeEmailAddress(User user)
        {
            var newEmailAddress = Guid.NewGuid().ToString("N").Substring(0, 8) + "@test.phundus.ch";
            ChangeEmailAddress(user, newEmailAddress);
        }

        private void ChangeEmailAddress(User user, string newEmailAddress, bool assertStatusCode = true)
        {
            _changedEmailAddressResult = App.ChangeEmailAddress(user.Guid, user.Password, newEmailAddress, assertStatusCode);
            if (_changedEmailAddressResult)
                user.RequestedEmailAddress = newEmailAddress;
        }

        [When(@"log in")]
        public void WhenLogIn()
        {
            var user = Ctx.User;
            Ctx.LoggedIn = App.LogIn(user.Username, user.Password, false);
        }

        [When(@"log in with requested address")]
        public void WhenLogInWithRequestedAddress()
        {
            var user = Ctx.User;
            Ctx.LoggedIn = App.LogIn(user.RequestedEmailAddress, user.Password, false);
        }

        [Then(@"can log in")]
        public void ThenCanLogIn()
        {
            var user = Ctx.User;
            App.LogIn(user.Username, user.Password);
        }

        [Then(@"logged in")]
        public void ThenLoggedIn()
        {
            Assert.That(Ctx.LoggedIn, Is.EqualTo(Ctx.User.Guid), "User not logged in.");
        }

        [Then(@"not logged in")]
        public void ThenNotLoggedIn()
        {
            Assert.That(Ctx.LoggedIn, Is.Not.EqualTo(Ctx.User.Guid));
        }

        [When(@"validate key")]
        public void WhenValidateKey()
        {
            _validateKeyResult = App.ValidateKey(Ctx.ValidationKey);
        }

        [Then(@"not validated")]
        public void ThenNotValidated()
        {
            Assert.IsFalse(_validateKeyResult);
        }

        [Then(@"error email address already taken")]
        public void ThenErrorEmailAddressAlreadyTaken()
        {
            // TODO: Exception handling
            Assert.False(_changedEmailAddressResult);
        }
    }
}