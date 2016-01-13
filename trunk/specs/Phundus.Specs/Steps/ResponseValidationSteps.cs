namespace Phundus.Specs.Steps
{
    using System.Net;
    using ContentTypes;
    using Machine.Specifications;
    using Newtonsoft.Json;
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
            get
            {
                return Resource.LastResponse;   
            }
        }

        private string TryGetErrorMessage()
        {
            return Resource.TryGetErrorMessage(LastResponse);
            
        }

        [Then(@"I should see error")]
        public void ThenIShouldSeeError()
        {
            Assert.That(LastResponse.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Then(@"I should see ok")]
        public void ThenIShouldSeeOk()
        {
            Assert.That(LastResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Then(@"I should see no content")]
        public void ThenIShouldSeeNoContent()
        {
            Assert.That(LastResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Then(@"I should see unauthorized")]
        public void ThenIShouldSeeUnauthorized()
        {
            Assert.That(LastResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Then(@"I should see message ""(.*)""")]
        public void ThenIShouldSeeMessage(string text)
        {            
            Assert.That(TryGetErrorMessage(), Is.EqualTo(text));
        }

        [Then(@"error email address already taken")]
        public void ThenErrorEmailAddressAlreadyTaken()
        {
            Assert.That(LastResponse.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            Assert.That(TryGetErrorMessage(), Is.StringContaining("EmailAlreadyTakenException"));
        }
    }
}