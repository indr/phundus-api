namespace Phundus.Specs.Steps
{
    using System.Net;
    using NUnit.Framework;
    using Services;
    using TechTalk.SpecFlow;

    [Binding]
    public class ResponseValidationSteps : StepsBase
    {
        public ResponseValidationSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Then(@"I should see error")]
        public void ThenIShouldSeeError()
        {
            Assert.That(App.LastResponse.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Then(@"I should see ok")]
        public void ThenIShouldSeeOk()
        {
            Assert.That(App.LastResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Then(@"I should see unauthorized")]
        public void ThenIShouldSeeUnauthorized()
        {
            Assert.That(App.LastResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

    }
}