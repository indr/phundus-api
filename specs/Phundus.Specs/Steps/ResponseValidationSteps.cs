namespace Phundus.Specs.Steps
{
    using NUnit.Framework;
    using Services;
    using TechTalk.SpecFlow;

    [Binding]
    public class ResponseValidationSteps : StepsBase
    {
        public ResponseValidationSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Then(@"last response code is (.*)")]
        public void ThenLastResponse(int statusCode)
        {
            Assert.That((int)App.LastResponse.StatusCode, Is.EqualTo(statusCode));
        }

    }
}