namespace Phundus.Specs.Services
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using Api;
    using NUnit.Framework;
    using Phundus.Rest.ContentObjects;
    using TechTalk.SpecFlow;

    [Binding]
    public class Ctx
    {
        public User User { get; set; }
    }

    [Binding]
    public class RestMailbox : AppBase
    {
        private readonly ApiClient _apiClient;

        public RestMailbox(ApiClient apiClient)
        {
            if (apiClient == null) throw new ArgumentNullException("apiClient");
            _apiClient = apiClient;
        }

        public Mail Find(string subject, string toAddress)
        {
            var response = _apiClient.For<MailsApi>().Query();
            AssertHttpStatus(HttpStatusCode.OK, response);

            return response.Data.Results.FirstOrDefault(p => p.Subject == subject && p.To.Contains(toAddress));
        }
    }
}