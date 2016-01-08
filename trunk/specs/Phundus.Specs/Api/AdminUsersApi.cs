namespace Phundus.Specs.Api
{
    using Phundus.Rest.Api.Admin;
    using RestSharp;

    public class AdminUsersApi : ApiBase
    {
        public AdminUsersApi()
            : base("admin/users/{userId}")
        {
        }

        public IRestResponse Patch(AdminUsersPatchRequestContent requestContent)
        {
            var request = GetRestRequest(requestContent, Method.PATCH);
            request.AddUrlSegment("userId", "0");
            return Execute(request);
        }
    }
}