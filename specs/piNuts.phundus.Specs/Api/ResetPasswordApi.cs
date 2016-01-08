namespace Phundus.Specs.Api
{
    using Phundus.Rest.Api.Account;
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

    public class ChangePasswordApi : ApiBase
    {
        public ChangePasswordApi() : base("account/change-password")
        {
        }
    }
}