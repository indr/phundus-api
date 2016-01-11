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

        [Then(@"response code is (.*)")]
        public void ThenResponse(int statusCode)
        {
            Assert.That((int)App.LastResponse.StatusCode, Is.EqualTo(statusCode));
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

    }
}