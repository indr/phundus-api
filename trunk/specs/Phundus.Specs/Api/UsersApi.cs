namespace Phundus.Specs.Api
{
    using Phundus.Rest.Api;
    using RestSharp;

    public class UsersApi : ApiBase
    {
        public UsersApi()
            : base("users")
        {
        }

        public IRestResponse<UsersPostOkResponseContent> Post(UsersPostRequestContent requestContent)
        {
            return Execute<UsersPostOkResponseContent>(requestContent, Method.POST);
        }
    }
}