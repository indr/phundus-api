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
        public void AngenommenBenutzerBestatigt()
        {
            var user = App.SignUpUser();
            App.ConfirmUser(user);
            Ctx.User = user;
        }
    }
}