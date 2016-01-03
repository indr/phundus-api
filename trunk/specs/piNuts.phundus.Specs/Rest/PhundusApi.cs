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

        public IRestResponse Exeucte2(RestRequest request)
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

        public IRestResponse<OrderDetailDoc> PostOrder(Guid organizationId, string username)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "orders";
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new {ownerId = organizationId, username = username});
            return Execute<OrderDetailDoc>(request);
        }

        public IRestResponse<OrderItemDoc> PostOrderItem(Guid organizationId, int orderId)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "orders/{orderId}/items";
            request.AddUrlSegment("orderId", orderId.ToString());
            request.RequestFormat = DataFormat.Json;
            request.AddBody(
                new
                {
                    articleId = 10020,
                    fromUtc = DateTime.UtcNow.Date,
                    toUtc = DateTime.UtcNow.AddDays(1).Date,
                    amount = 1
                });
            return Execute<OrderItemDoc>(request);
        }

        public IRestResponse<OrderDetailDoc> GetOrder(Guid organizationId, int orderId)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "orders/{orderId}";            
            request.AddUrlSegment("orderId", orderId.ToString());
            request.RequestFormat = DataFormat.Json;
            return Execute<OrderDetailDoc>(request);
        }

        public IRestResponse DeleteOrderItem(Guid organizationId, int orderId, Guid orderItemId)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "orders/{orderId}/items/{itemId}";
            request.AddUrlSegment("orderId", orderId.ToString());
            request.AddUrlSegment("itemId", orderItemId.ToString("D"));
            request.RequestFormat = DataFormat.Json;
            return Exeucte2(request);
        }

        public IRestResponse UpdateOrderItem(Guid organizationId, int orderId, Guid orderItemId, DateTime fromUtc, DateTime toUtc, int amount)
        {
            var request = new RestRequest(Method.PATCH);
            request.Resource = "orders/{orderId}/items/{itemId}";            
            request.AddUrlSegment("orderId", orderId.ToString());
            request.AddUrlSegment("itemId", orderItemId.ToString("D"));
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new {fromUtc = fromUtc, toUtc = toUtc, amount = amount});
            
            return Exeucte2(request);
        }

        public IRestResponse<OrderDetailDoc> PatchOrder(Guid organizationId, int orderId, string status)
        {
            var request = new RestRequest(Method.PATCH);
            request.Resource = "orders/{orderId}";            
            request.AddUrlSegment("orderId", orderId.ToString());
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new {status});
            return Execute<OrderDetailDoc>(request);
        }
    }
}