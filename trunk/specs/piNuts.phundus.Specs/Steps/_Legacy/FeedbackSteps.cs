namespace Phundus.Specs.Steps
{
    using TechTalk.SpecFlow;

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