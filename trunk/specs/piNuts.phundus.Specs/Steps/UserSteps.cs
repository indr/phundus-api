namespace Phundus.Specs.Steps
{
    using Services;
    using TechTalk.SpecFlow;

    [Binding]
    public class UserSteps : StepsBase
    {
        protected UserSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"bestätigter Benutzer")]
        public void AngenommenBestatigterBenutzer()
        {
            var user = App.SignUpUser();
            App.ConfirmUser(user.Guid);
            Ctx.User = user;
        }
    }
}