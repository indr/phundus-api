namespace Phundus.Specs.Rest
{
    using System;
    using System.Configuration;
    using RestSharp;

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

        public IRestResponse<OrderDetailDoc> PostOrder(int organizationId, string userName)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "organizations/{organizationId}/orders";
            request.AddUrlSegment("organizationId", organizationId.ToString());
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new {userName = userName});
            return Execute<OrderDetailDoc>(request);
        }
    }
}