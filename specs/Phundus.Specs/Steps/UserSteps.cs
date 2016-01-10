namespace Phundus.Specs.Steps
{
    using Services;
    using Services.Entities;
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

        [Given(@"not logged in")]
        public void GivenNotLoggedIn()
        {
            App.DeleteSessionCookies();
        }

        [Given(@"logged in as root")]
        public void GivenLoggedInAsRoot()
        {
            App.LogInAsRoot();
        }

        [Given(@"logged in as user")]
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

        [Given(@"a confirmed user")]
        public User AConfirmedUser()
        {
            var user = App.SignUpUser();
            App.LogInAsRoot();
            App.ConfirmUser(user.Guid);
            Ctx.User = user;
            return user;
        }

        [Given(@"a confirmed user ""([^@]*)""")]
        public void AConfirmedUser(string key)
        {
            Ctx.Users[key] = AConfirmedUser();
        }

        [Given(@"a confirmed and logged in user ""([^@]*)""")]
        public void AConfirmedAndLoggedInUser(string key)
        {
            var user = AConfirmedUser();
            App.LogIn(user.Username, user.Password);
            Ctx.Users[key] = user;
        }

        [Given(@"a confirmed user ""([^@]*)"" with email address ""(.*)""")]
        public void AConfirmedUser(string key, string email)
        {
            var user = AConfirmedUser();
            Ctx.Users[key] = user;
            Ctx.Emails[email] = user.EmailAddress;
        }

        [Given(@"a confirmed, locked user")]
        public void GivenAConfirmedLockedUser()
        {
            AConfirmedUser();
            App.LockUser(Ctx.User.Guid);
        }

        [Given(@"a confirmed admin")]
        public void GivenAConfirmedAdmin()
        {
            var user = App.SignUpUser();
            App.LogInAsRoot();
            App.ConfirmUser(user.Guid);
            App.SetUsersRole(user.Guid, UserRole.Admin);
            Ctx.User = user;
        }

        [Given(@"unlock user")]
        public void GivenUnlockUser()
        {
            App.LogInAsRoot();
            App.UnlockUser(Ctx.User.Guid);
            App.DeleteSessionCookies();
        }

        [Given(@"lock user")]
        public void GivenLockUser()
        {
            App.LogInAsRoot();
            App.LockUser(Ctx.User.Guid);
            App.DeleteSessionCookies();
        }
    }
}