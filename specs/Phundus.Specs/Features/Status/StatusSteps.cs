namespace Phundus.Specs.Features.Status
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using ContentTypes;
    using Machine.Specifications;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class StatusSteps : AppStepsBase
    {
        private StatusGetOkResponseContent _status;

        public StatusSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"I get status info")]
        public void WhenIGetStatusInfo()
        {
            _status = App.GetStatus();
        }

        [Then(@"it should have server url according to App\.config")]
        public void ThenItShouldHaveServerUrlAccordingToApp_Config()
        {
            var serverUrl = ConfigurationManager.AppSettings["ServerUrl"];
            if (serverUrl == "phundus.ch")
                serverUrl = "www.phundus.ch";
            Assert.That(_status.ServerUrl, Is.Not.Empty);
            Assert.That(_status.ServerUrl, Is.EqualTo(serverUrl));
        }

        [Then(@"it should have server date time within (.*) seconds?")]
        public void ThenItShouldHaveServerDateTimeWithinSecond(int seconds)
        {
            Assert.That(_status.ServerDateTimeUtc, Is.EqualTo(DateTime.UtcNow).Within(seconds).Seconds);
        }

        [Then(@"it should have server version according to specs assembly version")]
        public void ThenItShouldHaveServerVersionAccordingToSpecsAssemblyVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version.ToString(3);
            var revision = assembly.GetName().Version.Revision;
            var expected = String.Format("{0} (build {1})", version, revision);
            Assert.That(_status.ServerVersion, Is.EqualTo(expected));
        }

        [Then(@"I should see server status in maintenance (.*)")]
        public void ThenIShouldSeeServerStatusInMaintenance(bool inMaintenance)
        {
            if (_status == null)
                WhenIGetStatusInfo();
            Assert.That(_status.InMaintenance, Is.EqualTo(inMaintenance));
        }

    }
}