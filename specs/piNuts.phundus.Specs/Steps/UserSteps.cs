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

        [Given(@"a confirmed user")]
        public void AConfirmedUser()
        {
            var user = App.SignUpUser();
            App.ConfirmUser(user.Guid);
            Ctx.CurrentUser = user;
        }

        [Given(@"a confirmed admin")]
        public void AngenommenAConfirmedAdmin()
        {
            var user = App.SignUpUser();
            App.LogInAsRoot();
            App.ConfirmUser(user.Guid);
            App.SetUsersRole(user.Guid, UserRole.Admin);
            Ctx.CurrentUser = user;
        }

    }
}