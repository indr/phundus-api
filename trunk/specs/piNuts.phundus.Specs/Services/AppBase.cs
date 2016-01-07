namespace Phundus.Specs.Services
{
    using System.Net;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using RestSharp;

    public abstract class AppBase
    {
        protected void AssertHttpStatus(HttpStatusCode statusCode, IRestResponse restResponse)
        {
            var message = TryGetErrorMessage(restResponse);
            Assert.That(restResponse.StatusCode, Is.EqualTo(statusCode), message);
        }

        protected string TryGetErrorMessage(IRestResponse restResponse)
        {
            if (restResponse.ContentType != "application/json; charset=utf-8")
            {
                return null;
            }
            var errorContent = JsonConvert.DeserializeObject<ErrorContent>(restResponse.Content);
            return errorContent.Msg;
        }

        public class ErrorContent
        {
            [JsonProperty("message")]
            public string Msg { get; set; }
        }
    }
}