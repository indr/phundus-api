namespace Phundus.Specs.Api
{
    using Phundus.Rest.Api;
    using RestSharp;

    public class SessionsApi : ApiBase
    {
        public SessionsApi() : base("sessions")
        {
        }

        public IRestResponse<SessionsPostOkResponseContent> Post(SessionsPostRequestContent requestContent)
        {
            return Execute<SessionsPostOkResponseContent>(requestContent, Method.POST);
        }
    }
}