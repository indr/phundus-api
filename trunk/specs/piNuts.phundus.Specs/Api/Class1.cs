namespace Phundus.Rest.Specs
{
    using System;
    using System.Configuration;
    using System.Net;
    using Machine.Specifications;
    using RestSharp;

    public abstract class concern
    {
        protected static PhundusApi api = new PhundusApi("chief-1@test.phundus.ch", "1234");
    }

    [Subject("Contracts")]
    public class when_post_is_issued : concern
    {
        private static RestRequest request;
        private static IRestResponse<ContractDetailDoc> response;

        public Establish c = () => { };

        public Because of = () => { response = api.PostContract(1001, 10001); };

        public It should_return_status_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

        public It should_return_doc_with_contract_id = () => response.Data.ContractId.ShouldBeGreaterThan(0);

        public It should_return_doc_with_created_on =
            () => response.Data.CreatedOn.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));

        public It should_return_doc_with_organization_id = () => response.Data.OrganizationId.ShouldEqual(1001);
    }

    public class PhundusApi
    {
        static PhundusApi()
        {
            var baseUrl = ConfigurationManager.AppSettings["ServerUrl"];
            BaseUrl = "http://" + baseUrl + "/api";
        }

        private static readonly string BaseUrl;

        private readonly string _password;
        private readonly string _userName;

        public PhundusApi(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        public IRestResponse Exeucte(RestRequest request)
        {
            var client = CreateClient();
            var response = client.Execute(request);
            HandleException(response.ErrorException);
            return response;
        }

        public IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            var client = CreateClient();
            var response = client.Execute<T>(request);
            HandleException(response.ErrorException);
            return response;
        }

        private RestClient CreateClient()
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl;
            //client.Authenticator = new HttpBasicAuthenticator(_userName, _password);
            client.Authenticator = new PhundusApiAuthenticator(_userName, _password);
            return client;
        }

        private void HandleException(Exception ex)
        {
            if (ex == null)
                return;

            const string message = "Error retrieving response. Check inner details for more info.";
            throw new ApplicationException(message, ex);
        }

        public class ContractsPostDto
        {
            public int UserId { get; set; }
        }

        public IRestResponse<ContractDetailDoc> PostContract(int organizationId, int userId)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "organizations/{organizationId}/contracts";
            request.AddUrlSegment("organizationId", organizationId.ToString());
            
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new {userId = userId});
            return Execute<ContractDetailDoc>(request);
        }
    }

    public class ContractDetailDoc
    {
        public int ContractId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class PhundusApiAuthenticator : IAuthenticator
    {
        public class SessionDoc
        {
            public string Password { get; set; }
            public string Username { get; set; }
        }

        private readonly string _password;
        private readonly string _userName;

        public PhundusApiAuthenticator(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (request.Resource == "sessions")
                return;

            var r = new RestRequest("sessions", Method.POST);
            r.AddBody(new SessionDoc {Username = _userName, Password = _password});
            r.UserState = "MyAuth";
            var re = client.Execute(r);


            foreach (var each in re.Cookies)
                request.AddCookie(each.Name, each.Value);
        }
    }
}