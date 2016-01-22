namespace Phundus.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Configuration;
    using System.Text.RegularExpressions;
    using ContentTypes;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using RestSharp;
    using RestSharp.Deserializers;

    public class Resource
    {
        private static readonly string BaseUrl;

        private static readonly IDictionary<string, string> Cookies = new Dictionary<string, string>();
        private readonly string _resource;
        private bool _assertHttpStatusCode;

        public static IRestResponse LastResponse { get; private set; }

        static Resource()
        {
            var baseUrl = ConfigurationManager.AppSettings["ServerUrl"];

            if (baseUrl == "phundus.ch")
                BaseUrl = "https://www.phundus.ch/api/v0/";
            else
                BaseUrl = "http://" + baseUrl + "/api/v0/";
        }

        public Resource(string resource, bool assertHttpStatusCode)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            _resource = resource;
            _assertHttpStatusCode = assertHttpStatusCode;
        }

        public static void DeleteSessionCookies()
        {
            Cookies.Clear();
        }

        public IRestResponse<T> Get<T>(object requestContent, object queryParams = null) where T : new()
        {
            var request = CreateRequest(requestContent, Method.GET, queryParams);
            return Execute<T>(request);
        }

        public IRestResponse Delete()
        {
            var request = CreateRequest(null, Method.DELETE);
            return Execute(request);
        }

        public IRestResponse Delete(object requestContent)
        {
            var request = CreateRequest(requestContent, Method.DELETE);
            return Execute(request);
        }

        public IRestResponse Patch(object requestContent)
        {
            var request = CreateRequest(requestContent, Method.PATCH);
            return Execute(request);
        }

        public IRestResponse Post(object requestContent)
        {
            var request = CreateRequest(requestContent, Method.POST);
            return Execute(request);
        }

        #region PostFile
        public TResponseContent PostFile<TResponseContent>(object requestContent, string fullFileName, string fileName)
        {
            // https://stackoverflow.com/questions/14143630/upload-file-through-c-sharp-using-json-request-and-restsharp

            fileName = fileName ?? Path.GetFileName(fullFileName);

            //string path = @"C:\Projectos\My Training Samples\Adobe Sample\RBO1574.pdf";
            string path = fullFileName;
            //localhost settings
            //string requestHost = @"http://localhost:3000/receipts";
            string requestHost = BaseUrl + ReplaceUrlSegments(_resource, requestContent);
            string tagnr = "p94tt7w";
            string machinenr = "2803433";
            string safe_token = "123";

            FileStream fs1 = File.OpenRead(path);
            long filesize = fs1.Length;
            fs1.Close();

            //Debug.WriteLine(path + " is " + filesize + " bytes");

            // Create a http request to the server endpoint that will pick up the
            // file and file description.
            HttpWebRequest requestToServerEndpoint =
                (HttpWebRequest)WebRequest.Create(requestHost);

            //string boundaryString = "FFF3F395A90B452BB8BEDC878DDBD152";
            string boundaryString = "---------------------------82158952720152522822137219797";
            string fileUrl = path;

            // Set the http request header \\
            // multipart/form-data; boundary=---------------------------82158952720152522822137219797
            requestToServerEndpoint.Method = WebRequestMethods.Http.Post;
            requestToServerEndpoint.ContentType = "multipart/form-data; boundary=" + boundaryString;
            requestToServerEndpoint.KeepAlive = true;
            //requestToServerEndpoint.Credentials = System.Net.CredentialCache.DefaultCredentials;
            requestToServerEndpoint.Accept = "application/json";

            var cookies = Cookies.Select(p => p.Key + "=" + p.Value);
            var cookie = String.Join("=", cookies);
            requestToServerEndpoint.Headers["Cookie"] = cookie;


            // Use a MemoryStream to form the post data request,
            // so that we can get the content-length attribute.
            MemoryStream postDataStream = new MemoryStream();
            StreamWriter postDataWriter = new StreamWriter(postDataStream);

            //// Include value from the tag_number text area in the post data
            //postDataWriter.Write("\r\n--" + boundaryString + "\r\n");
            //postDataWriter.Write("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}",
            //                        "receipt[tag_number]",
            //                        tagnr);

            //// Include ispaperduplicate text area in the post data
            //postDataWriter.Write("\r\n--" + boundaryString + "\r\n");
            //postDataWriter.Write("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}",
            //                        "receipt[ispaperduplicate]",
            //                        0);

            //// Include value from the machine number in the post data
            //postDataWriter.Write("\r\n--" + boundaryString + "\r\n");
            //postDataWriter.Write("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}",
            //                        "machine[serial_number]",
            //                        machinenr);

            //// Include value from the machine token in the post data
            //postDataWriter.Write("\r\n--" + boundaryString + "\r\n");
            //postDataWriter.Write("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}",
            //                        "machine[safe_token]",
            //                        safe_token);

            // Include the file in the post data
            /*
             Content-Disposition: form-data; name="files[]"; filename="12365935_1736670393241540_1626625293305475497_o.jpg"
             Content-Type: image/jpeg
             */
            postDataWriter.Write("--" + boundaryString + "\r\n");
            postDataWriter.Write("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n"
                //+ "Content-Length: \"{2}\"\r\n"
                                    + "Content-Type: image/{2}\r\n"
                //+ "Content-Transfer-Encoding: binary\r\n\r\n"
                                    , "files[]"
                                    , fileName
                                    , Path.GetExtension(fileName).Trim('.')
                                    );
            postDataWriter.Write("\r\n");
            postDataWriter.Flush();


            // Read the file
            FileStream fileStream = new FileStream(fileUrl, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                postDataStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            postDataWriter.Write("\r\n--" + boundaryString + "--\r\n");
            postDataWriter.Flush();

            // Set the http request body content length
            requestToServerEndpoint.ContentLength = postDataStream.Length;
            //Debug.WriteLine("ContentLength: " + postDataStream.Length);

            // Dump the post data from the memory stream to the request stream
            Stream s = requestToServerEndpoint.GetRequestStream();

            postDataStream.WriteTo(s);
            postDataStream.Flush();
            postDataStream.Close();


            var response = (HttpWebResponse)requestToServerEndpoint.GetResponse();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var stream = response.GetResponseStream();
            var streamReader = new StreamReader(stream);
            var jsonTextReader = new JsonTextReader(streamReader);

        
            var settings = new JsonSerializerSettings();
            
            return JsonSerializer.Create(settings).Deserialize<TResponseContent>(jsonTextReader);

        } 
        #endregion

        public IRestResponse<T> Post<T>(object requestContent) where T : new()
        {
            var request = CreateRequest(requestContent, Method.POST);
            return Execute<T>(request);
        }

        public IRestResponse<QueryOkResponseContent<T>> Query<T>(object queryParams = null) where T : new()
        {
            var request = CreateQueryRequest(queryParams, Method.GET);
            return Execute<QueryOkResponseContent<T>>(request);
        }

        protected RestRequest CreateRequest(object requestContent, Method method, object queryString = null)
        {
            var request = new RestRequest(method);
            request.Resource = _resource;
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new CustomJsonSerializer();
            AddUrlSegments(request, requestContent);
            AddQueryString(request, queryString);
            if (requestContent != null)
                request.AddBody(requestContent);
            foreach (var each in Cookies)
            {
                request.AddCookie(each.Key, each.Value);
            }
            return request;
        }

        
        private string ReplaceUrlSegments(string url, object requestContent)
        {
            var regex = new Regex(@"\/\{([a-z]+)\}", RegexOptions.IgnoreCase);
            var match = regex.Match(url);
            while (match.Success)
            {
                var name = match.Groups[1].Value;
                var value = GetParamValue(requestContent, name);
                url = url.Remove(match.Groups[0].Index, match.Groups[0].Length);
                if (value != null)
                    url = url.Insert(match.Groups[0].Index, "/" + value);
                
                var startAt = match.Groups[0].Index + (value != null ? value.Length : 0);
                match = regex.Match(url, startAt);
            }
            return url;
        }

        private void AddUrlSegments(RestRequest request, object requestContent)
        {
            var regex = new Regex(@"\/\{([a-z]+)\}", RegexOptions.IgnoreCase);
            var match = regex.Match(request.Resource);
            while (match.Success)
            {
                var startAt = match.Groups[0].Index + match.Groups[0].Length;
                if (!AddUrlSegment(request, requestContent, match.Groups[1].Value))
                {
                    RemoveUrlSegment(request, match.Groups[0]);
                    startAt = match.Groups[0].Index;
                }
                match = regex.Match(request.Resource, startAt);
            }
        }

        private void RemoveUrlSegment(RestRequest request, Group @group)
        {
            request.Resource = request.Resource.Remove(@group.Index, @group.Length);
        }

        private bool AddUrlSegment(RestRequest request, object requestContent, string name)
        {
            if (requestContent == null)
                return false;
            var value = GetParamValue(requestContent, name);
            if (value != null)
                request.AddUrlSegment(name, value);            
            return value != null;
        }

        private string GetParamValue(Object requestContent, string name)
        {
            string result = null;
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

                if (propertyInfo.Name[0].ToString().ToLowerInvariant() == name[0].ToString(CultureInfo.InvariantCulture)
                    && propertyInfo.Name.Substring(1) == name.Substring(1))
                {
                    var value = propertyInfo.GetValue(requestContent, null);
                    if (value != null)
                        result = value.ToString();
                }
            }
            return result;
        }

        private void AddQueryString(RestRequest request, object queryString)
        {
            if (queryString == null)
                return;
            foreach (var each in queryString.GetType().GetProperties())
            {
                request.AddParameter(each.Name, each.GetValue(queryString, null), ParameterType.QueryString);
            }
        }


        protected RestRequest CreateQueryRequest(object queryParams, Method method)
        {
            var request = CreateRequest(queryParams, Method.GET);            
            AddQueryParameters(request, queryParams);
            return request;
        }

        private void AddQueryParameters(RestRequest request, object queryParams)
        {
            if (queryParams == null)
                return;

            foreach (var propertyInfo in queryParams.GetType().GetProperties())
            {
                request.AddParameter(propertyInfo.Name, propertyInfo.GetValue(queryParams, null).ToString());
            }
        }

        private IRestResponse Execute(RestRequest request)
        {
            var response = CreateClient().Execute(request);
            HandleErrorException(response);
            if (_assertHttpStatusCode)
                AssertHttpStatusCodeIs2xx(response);
            ReadCookies(response);
            SetLastResponse(response);
            return response;
        }

        

        private IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            var response = CreateClient().Execute<T>(request);
            HandleErrorException(response);
            if (_assertHttpStatusCode)
                AssertHttpStatusCodeIs2xx(response);
            ReadCookies(response);
            SetLastResponse(response);
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

        private void AssertHttpStatusCodeIs2xx(IRestResponse response)
        {
            var message = TryGetErrorMessage(response);
            Assert.That((int)response.StatusCode, Is.InRange(200, 299), message);
        }

        private static void ReadCookies(IRestResponse response)
        {
            foreach (var each in response.Cookies)
            {
                Cookies[each.Name] = each.Value;
            }
        }

        private static void SetLastResponse(IRestResponse response)
        {
            LastResponse = response;
        }

        public static string TryGetErrorMessage(IRestResponse lastResponse)
        {
            var restResponse = lastResponse;
            if (restResponse.ContentType != "application/json; charset=utf-8")
            {
                return null;
            }
            var errorContent = JsonConvert.DeserializeObject<ErrorContent>(restResponse.Content);
            return errorContent.Msg;
        }

    }
}