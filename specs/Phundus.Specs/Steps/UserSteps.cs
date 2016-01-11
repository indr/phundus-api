namespace Phundus.Specs.Steps
{
    using Services;
    using TechTalk.SpecFlow;

    public enum UserRole
    {
        Admin = 2
    }

    [Binding]
    public class UserSteps : StepsBase
    {
        public UserSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"I am logged in as root")]
        public void GivenIAmLoggedInAsRoot()
        {
            App.LogInAsRoot();
        }

        [Given(@"logged in as root")]
        public void GivenLoggedInAsRoot()
        {
            App.LogInAsRoot();
        }

        [Given(@"I am logged in as a user")]
        public void GivenIAmLoggedInAsAUser()
        {
            AConfirmedUser();
            GivenLoggedInAsUser();
        }

        [Given(@"logged in")]
        public void GivenLoggedInAsUser()
        {
            var user = Ctx.User;
            App.LogIn(user.Username, user.Password);
        }

        [Given(@"logged in as ""(.*)""")]
        public void GivenLoggedInAs(string userKey)
        {
            var user = Ctx.Users[userKey];
            App.LogIn(user.Username, user.Password);
        }

        [Given(@"I am logged in as ""(.*)""")]
        public void GivenIAmLoggedInAs(string userKey)
        {
            var user = Ctx.Users[userKey];
            App.LogIn(user.Username, user.Password);
            Ctx.User = user;
        }

        [Given(@"a confirmed user")]
        public void AConfirmedUser()
        {
            AConfirmedUser(null);
        }

        [Given(@"a confirmed user ""([^@]*)""")]
        public void AConfirmedUser(string userKey)
        {
            AConfirmedUser(userKey, null);
        }

        [Given(@"a confirmed user ""([^@]*)"" with email address ""(.*)""")]
        public void AConfirmedUser(string userKey, string emailKey)
        {
            string emailAddress = null;
            if (emailKey != null)
                Ctx.Emails.TryGetValue(emailKey, out emailAddress);
            var user = App.SignUpUser(emailAddress);
            App.ConfirmUser(user.Guid);

            Ctx.User = user;
            if (userKey != null)
                Ctx.Users[userKey] = user;
            if (emailKey != null)
                Ctx.Emails[emailKey] = user.EmailAddress;
        }

        [Given(@"a confirmed and logged in user ""([^@]*)""")]
        public void AConfirmedAndLoggedInUser(string userKey)
        {
            AConfirmedUser(userKey);
            var user = Ctx.Users[userKey];
            App.LogIn(user.Username, user.Password);
        }

        [Given(@"unlock user")]
        public void GivenUnlockUser()
        {
            App.UnlockUser(Ctx.User.Guid);
        }

        [Given(@"lock user")]
        public void GivenLockUser()
        {
            App.LockUser(Ctx.User.Guid);
        }
    }
}