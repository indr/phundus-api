namespace Phundus.Specs.Api
{
    using Phundus.Rest.ContentObjects;
    using RestSharp;

    public class MailsApi : ApiBase
    {
        public MailsApi() : base("/mails")
        {
        }

        public IRestResponse<QueryOkResponseContent<Mail>> Query()
        {
            var request = GetRestRequest(Method.GET);
            return Execute<QueryOkResponseContent<Mail>>(request);
        }
    }
}