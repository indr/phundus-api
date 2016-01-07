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

        [When(@"Passwort zurücksetzen")]
        public void WennPasswortZurucksetzen()
        {
            
            App.ResetPassword(Ctx.User);
        }

        [Then(@"E-Mail ""(.*)"" an Benutzer")]
        public void DannE_MailAnBenutzer(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}