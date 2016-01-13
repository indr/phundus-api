namespace Phundus.Specs.Steps
{
    using System.Net;
    using NUnit.Framework;
    using RestSharp;
    using Services;
    using TechTalk.SpecFlow;

    [Binding]
    public class ResponseValidationSteps : StepsBase
    {
        public ResponseValidationSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        private IRestResponse LastResponse
        {
            get { return Resource.LastResponse; }
        }

        private string TryGetErrorMessage()
        {
            return Resource.TryGetErrorMessage(LastResponse);
        }

        private void AssertLastStatusCode(HttpStatusCode statusCode)
        {
            Assert.That(LastResponse.StatusCode, Is.EqualTo(statusCode));
        }

        private void AssertLastMessage(string messagePart)
        {
            Assert.That(TryGetErrorMessage(), Is.StringContaining(messagePart));
        }

        [Then(@"I should see error")]
        public void ThenIShouldSeeError()
        {
            AssertLastStatusCode(HttpStatusCode.InternalServerError);
        }

        [Then(@"I should see ok")]
        public void ThenIShouldSeeOk()
        {
            AssertLastStatusCode(HttpStatusCode.OK);
        }

        [Then(@"I should see no content")]
        public void ThenIShouldSeeNoContent()
        {
            AssertLastStatusCode(HttpStatusCode.NoContent);
        }

        [Then(@"I should see not found")]
        public void ThenIShouldSeeNotFound()
        {
            AssertLastStatusCode(HttpStatusCode.NotFound);
        }


        [Then(@"I should see unauthorized")]
        public void ThenIShouldSeeUnauthorized()
        {
            AssertLastStatusCode(HttpStatusCode.Unauthorized);
        }

        [Then(@"I should see message ""(.*)""")]
        public void ThenIShouldSeeMessage(string text)
        {
            Assert.That(TryGetErrorMessage(), Is.EqualTo(text));
        }

        [Then(@"error email address already taken")]
        public void ThenErrorEmailAddressAlreadyTaken()
        {
            AssertLastStatusCode(HttpStatusCode.InternalServerError);
            AssertLastMessage("EmailAlreadyTakenException");
        }
    }
}