using TechTalk.SpecFlow;

namespace piNuts.phundus.Specs.Steps
{
    [Binding]
    public class LoginSteps : StepBase
    {
        [Given(@"ich bin auf der Loginseite")]
        public void AngenommenIchBinAufDerLoginseite()
        {
            Browser.GoTo(BaseUrl + "/#/login");
        }
    }
}