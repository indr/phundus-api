namespace Phundus.Specs.Api
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json;
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

        public static void DeleteSessionCookies()
        {
            Cookies.Clear();
        }

        public IRestResponse Patch(object requestContent)
        {
            var request = CreateRestRequest(requestContent, Method.PATCH);
            return Execute(request);
        }

        public IRestResponse Post(object requestContent)
        {
            var request = CreateRestRequest(requestContent, Method.POST);
            return Execute(request);
        }

        public IRestResponse<T> Post<T>(object requestContent) where T : new()
        {
            var request = CreateRestRequest(requestContent, Method.POST);
            return Execute<T>(request);
        }

        public IRestResponse<T> Query<T>() where T : new()
        {
            var request = CreateRestRequest(null, Method.GET);
            return Execute<T>(request);
        }

        protected RestRequest CreateRestRequest(object requestContent, Method method)
        {
            var request = new RestRequest(method);
            request.Resource = _resource;
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new CustomJsonSerializer();
            AddUrlSegments(request, requestContent);
            if (requestContent != null)
                request.AddBody(requestContent);
            foreach (var each in Cookies)
            {
                request.AddCookie(each.Key, each.Value);
            }
            return request;
        }

        private void AddUrlSegments(RestRequest request, object requestContent)
        {
            var regex = new Regex(@"\{([a-z]+)\}", RegexOptions.IgnoreCase);
            var match = regex.Match(request.Resource);
            while (match.Success)
            {
                AddUrlSegment(request, requestContent, match.Groups[1].Value);
                match = match.NextMatch();
            }
        }

        private void AddUrlSegment(RestRequest request, object requestContent, string name)
        {
            var value = GetParamValue(requestContent, name);
            request.AddUrlSegment(name, value);
        }

        private string GetParamValue(Object requestContent, string name)
        {
            foreach (var propertyInfo in requestContent.GetType().GetProperties())
            {
                foreach (
                    JsonPropertyAttribute attr in
                        propertyInfo.GetCustomAttributes(typeof (JsonPropertyAttribute), false))
                {
                    if (attr.PropertyName != name)
                        continue;
                    var value = propertyInfo.GetValue(requestContent, null);
                    if (value == null)
                        return "";
                    return value.ToString();
                }
            }
            return "";
        }

        private static IRestResponse Execute(RestRequest request)
        {
            var response = CreateClient().Execute(request);
            HandleErrorException(response);
            ReadCookies(response);
            return response;
        }

        private static IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            var response = CreateClient().Execute<T>(request);
            HandleErrorException(response);
            ReadCookies(response);
            return response;
        }

        private static RestClient CreateClient()
        {
            return new RestClient(BaseUrl);
        }

        private static void HandleErrorException(IRestResponse response)
        {
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
        }

        private static void ReadCookies(IRestResponse response)
        {
            foreach (var each in response.Cookies)
            {
                Cookies[each.Name] = each.Value;
            }
        }
    }
}