namespace Phundus.Specs.Api
{
    using Phundus.Rest.Api;
    using Phundus.Rest.Api.Admin;
    using Phundus.Rest.ContentObjects;
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