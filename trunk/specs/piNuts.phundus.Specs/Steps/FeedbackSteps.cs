using TechTalk.SpecFlow;

namespace piNuts.phundus.Specs.Steps
{
    [Binding]
    public class FeedbackSteps : StepBase
    {
        [Given(@"ich bin auf der Feedbackseite")]
        public void AngenommenIchBinAufDerFeedbackseite()
        {
            Browser.GoTo(BaseUrl + "/feedback");
        }
    }
}