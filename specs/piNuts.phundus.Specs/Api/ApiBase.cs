namespace Phundus.Specs.Api
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using RestSharp;

    public abstract class ApiBase
    {
        private static readonly string BaseUrl;

        private static readonly IDictionary<string, string> Cookies = new Dictionary<string, string>();
        private readonly string _resource;

        static ApiBase()
        {
            var baseUrl = ConfigurationManager.AppSettings["ServerUrl"];
            BaseUrl = "http://" + baseUrl + "/api/v0";
        }

        protected ApiBase(string resource)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            _resource = resource;
        }

        protected IRestResponse Execute(Object requestContent, Method method)
        {
            var request = GetRestRequest(requestContent, method);
            var response = Execute(request);
            return response;
        }

        protected IRestResponse Execute(RestRequest request)
        {
            var response = GetClient().Execute(request);
            HandleErrorException(response);
            ReadCookies(response);
            return response;
        }

        protected IRestResponse<T> Execute<T>(Object requestContent, Method method) where T : new()
        {
            var request = GetRestRequest(requestContent, method);
            var response = Execute<T>(request);
            return response;
        }

        protected IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            var response = GetClient().Execute<T>(request);
            HandleErrorException(response);
            ReadCookies(response);
            return response;
        }

        private static void ReadCookies(IRestResponse response)
        {
            foreach (var each in response.Cookies)
            {
                Cookies[each.Name] = each.Value;
            }
        }

        private static RestClient GetClient()
        {
            return new RestClient(BaseUrl);
        }

        protected RestRequest GetRestRequest(Method method)
        {
            return GetRestRequest(null, method);
        }

        protected RestRequest GetRestRequest(object requestContent, Method method)
        {
            var request = new RestRequest(method);
            request.Resource = _resource;
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new CustomJsonSerializer();
            if (requestContent != null)
                request.AddBody(requestContent);
            foreach (var each in Cookies)
            {
                request.AddCookie(each.Key, each.Value);
            }
            return request;
        }

        private static void HandleErrorException(IRestResponse response)
        {
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
        }

        public IRestResponse<T> Post<T>(object requestContent) where T : new()
        {
            var request = GetRestRequest(requestContent, Method.POST);
            return Execute<T>(request);
        }

        public IRestResponse<T> Query<T>() where T : new()
        {
            var request = GetRestRequest(Method.GET);
            return Execute<T>(request);
        }

        public static void DeleteSessionCookies()
        {
            Cookies.Clear();
        }
    }
}