namespace Phundus.Specs.Api
{
    using Phundus.Rest.ContentObjects;
    using RestSharp;

    public class ResetPasswordApi : ApiBase
    {
        public ResetPasswordApi() : base("account/reset-password")
        {
        }

        public IRestResponse Post(ResetPasswordPostRequestContent requestContent)
        {
            return Execute(requestContent, Method.POST);
        }
    }
}