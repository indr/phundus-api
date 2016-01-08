namespace Phundus.Specs.Features.Feedback
{
    using System;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class FeedbackSteps : StepsBase
    {
        public FeedbackSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"submit feedback as anon")]
        public void WennSubmitFeedbackAsAnon()
        {
            Ctx.AnonEmailAddress = Guid.NewGuid().ToString("N").Substring(0, 8) + "@test.phundus.ch";
            App.SendFeedback(Ctx.AnonEmailAddress);
        }
    }
}