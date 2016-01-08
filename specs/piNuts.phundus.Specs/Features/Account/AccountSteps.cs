namespace Phundus.Specs.Steps
{
    using Services;
    using TechTalk.SpecFlow;

    [Binding]
    public class AccountSteps : StepsBase
    {
        public AccountSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"reset password")]
        public void ResetPassword()
        {
            App.ResetPassword(Ctx.CurrentUser.EmailAddress);
        }
    }
}