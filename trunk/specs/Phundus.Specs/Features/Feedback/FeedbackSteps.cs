namespace Phundus.Specs.Features.Feedback
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class FeedbackSteps : AppStepsBase
    {
        public FeedbackSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"submit feedback as anon with comment:")]
        public void WhenSubmitFeedbackAsAnonWithComment(string comment)
        {
            comment = comment.Replace("{AppSettings.ServerUrl}", ConfigurationManager.AppSettings["ServerUrl"]);
            comment = comment.Replace("{Assembly.Version}", Assembly.GetExecutingAssembly().GetName().Version.ToString());

            Ctx.EmailAddress = Guid.NewGuid().ToString("N").Substring(0, 8) + "@test.phundus.ch";
            App.SendFeedback(Ctx.EmailAddress, comment);
        }

        [When(@"I try to submit feedback with email address ""(.*)""")]
        public void WhenITrytoSubmitFeedbackWithEmailAddress(string emailAddress)
        {
            App.SendFeedback(emailAddress, "");
        }
    }
}