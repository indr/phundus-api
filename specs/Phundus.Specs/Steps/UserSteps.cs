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

        [Given(@"a confirmed user")]
        public void AConfirmedUser()
        {
            var user = App.SignUpUser();
            App.LogInAsRoot();
            App.ConfirmUser(user.Guid);
            Ctx.User = user;
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