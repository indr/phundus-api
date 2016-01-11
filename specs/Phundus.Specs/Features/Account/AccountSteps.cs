namespace Phundus.Specs.Features.Account
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using NUnit.Framework;
    using Services;
    using Services.Entities;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class AccountSteps : StepsBase
    {
        private readonly EmailSteps _emailSteps;
        private readonly UserSteps _userSteps;

        public AccountSteps(App app, Ctx ctx, EmailSteps emailSteps, UserSteps userSteps) : base(app, ctx)
        {
            if (emailSteps == null) throw new ArgumentNullException("emailSteps");
            if (userSteps == null) throw new ArgumentNullException("userSteps");
            _emailSteps = emailSteps;
            _userSteps = userSteps;
        }

        [When(@"sign up")]
        public void WhenSignUp()
        {
            var user = App.SignUpUser();
            Ctx.User = user;
        }

        [Given(@"I signed up")]
        public void GivenISignedUp()
        {
            var user = App.SignUpUser();
            Ctx.User = user;
        }

        [Given(@"I got the validation key from account validation email")]
        public void GivenIGotTheValidationKeyFromAccountValidationEmail()
        {
            _emailSteps.GivenTheValidationKeyFromEmail();
        }

        [Given(@"I validated the key")]
        public void GivenIValidatedTheKey()
        {
            App.ValidateKey(Ctx.ValidationKey);
        }

        [When(@"I try to log in")]
        public void WhenITryToLogIn()
        {
            var user = Ctx.User;
            Ctx.LoggedIn = App.LogIn(user.Username, user.Password, false);
        }

        [Then(@"I should be logged in")]
        public void ThenIShouldBeLoggedIn()
        {
            Assert.That(Ctx.LoggedIn, Is.EqualTo(Ctx.User.Guid), "User not logged in.");
        }

        [When(@"I try to reset user's password")]
        public void WhenITryToResetPasswordUserSPassword()
        {
            App.ResetPassword(Ctx.User.Username);
        }

        [When(@"I try to sign up")]
        public void WhenITryToSignUp()
        {
            var user = App.SignUpUser();
            Ctx.User = user;
        }

        [Then(@"I should receive email ""(.*)""")]
        public void ThenIShouldReceiveEmail(string subject)
        {
            _emailSteps.ThenUserShouldReceiveEmail(subject);
        }
        
        [Given(@"user changed email address")]
        public void GivenUserChangedEmailAddress()
        {
            ChangeEmailAddress(Ctx.User);
        }

        [Given(@"I changed my email address")]
        public void GivenIChangedMyEmailAddress()
        {
            ChangeEmailAddress(Ctx.User);
        }

        [Given(@"""(.*)"" changed email address to ""(.*)""")]
        public void GivenUserChangedEmailAddress(string userKey, string emailKey)
        {
            var user = Ctx.Users[userKey];
            if (!Ctx.Emails.ContainsAlias(emailKey))
                Ctx.Emails[emailKey] = Guid.NewGuid().ToString("N").Substring(0, 8) + "@test.phundus.ch";
            var emailAddress = Ctx.Emails[emailKey];
            ChangeEmailAddress(user, emailAddress);
        }

        [Given(@"I changed email address to ""(.*)""")]
        public void GivenIChangedEmailAddressTo(string emailKey)
        {
            var user = Ctx.User;
            if (!Ctx.Emails.ContainsAlias(emailKey))
                Ctx.Emails[emailKey] = Guid.NewGuid().ToString("N").Substring(0, 8) + "@test.phundus.ch";
            var emailAddress = Ctx.Emails[emailKey];
            ChangeEmailAddress(user, emailAddress);
        }


        [When(@"change email address")]
        public void WhenChangeEmailAddress()
        {
            ChangeEmailAddress(Ctx.User);
        }

        [When(@"I try to change my email address")]
        public void WhenIChangeEmailAddress()
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

        [When(@"I try to change my email address to ""(.*)""")]
        public void WhenITryToChangeMyEmailAddressTo(string emailKey)
        {
            var user = Ctx.User;
            var emailAddress = Ctx.Emails[emailKey];

            ChangeEmailAddress(user, emailAddress, false);
        }


        private void ChangeEmailAddress(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var newEmailAddress = Guid.NewGuid().ToString("N").Substring(0, 8) + "@test.phundus.ch";
            ChangeEmailAddress(user, newEmailAddress);
        }

        private void ChangeEmailAddress(User user, string newEmailAddress, bool assertStatusCode = true)
        {
            if (user == null) throw new ArgumentNullException("user");

            App.ChangeEmailAddress(user.Guid, user.Password, newEmailAddress, assertStatusCode);
            if (App.LastResponse.IsSuccess)
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

        [When(@"I try to log in with requested address")]
        public void WhenITryToLogInWithRequestedAddress()
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

        [When(@"validate key")]
        public void WhenValidateKey()
        {
            Debug.WriteLine(String.Format("Validating key {0}", Ctx.ValidationKey));
            App.ValidateKey(Ctx.ValidationKey, false);
        }

        [When(@"I try to validate the key")]
        public void WhenITryToValidateTheKey()
        {
            Debug.WriteLine(String.Format("Validating key {0}", Ctx.ValidationKey));
            App.ValidateKey(Ctx.ValidationKey, false);
        }

        
        [Then(@"not validated")]
        public void ThenNotValidated()
        {
            Assert.That(App.LastResponse.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError), "Error validating key " + Ctx.ValidationKey);
            Assert.That(App.LastResponse.Message, Is.StringStarting("could not update"), "Error validating key " + Ctx.ValidationKey);
        }

        [Then(@"error email address already taken")]
        public void ThenErrorEmailAddressAlreadyTaken()
        {
           Assert.That(App.LastResponse.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
           Assert.That(App.LastResponse.Message, Is.StringContaining("EmailAlreadyTakenException"));
        }

        [Given(@"I signed up and confirmed my email address")]
        public void GivenISignedUpAndConfirmedMyEmailAddress()
        {
            _userSteps.AConfirmedUser();
        }

        [Then(@"I should not be logged in")]
        public void ThenIShouldNotBeLoggedIn()
        {
            Assert.That(Ctx.LoggedIn, Is.Not.EqualTo(Ctx.User.Guid));
        }

        [Given(@"an administrator locked my account")]
        public void GivenAnAdministratorLockedMyAccount()
        {
            App.LockUser(Ctx.User.Guid);
        }

        [Given(@"an administrator unlocked my account")]
        public void GivenAnAdministratorUnlockedMyAccount()
        {
            App.UnlockUser(Ctx.User.Guid);
        }

        [Given(@"I changed my password")]
        public void GivenIChangedMyPassword()
        {
            var newPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
            var user = Ctx.User;
            App.ChangePassword(user.Guid, user.Password, newPassword);
            user.Password = newPassword;
        }

        [When(@"I try to login with my new password")]
        public void WhenITryToLoginWithMyNewPassword()
        {
            WhenITryToLogIn();
        }

        [When(@"I try to login with my old password")]
        public void WhenITryToLoginWithMyOldPassword()
        {
            var user = Ctx.User;
            Ctx.LoggedIn = App.LogIn(user.Username, user.OldPassword, false);
        }

    }
}