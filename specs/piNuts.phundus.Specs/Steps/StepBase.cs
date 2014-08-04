namespace piNuts.phundus.Specs.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.InteropServices;
    using System.Text;
    using Browsers;
    using WatiN.Core;

    public class StepBase
    {
        protected static IeBrowser Browser
        {
            get { return Browsers.Browser.Current; }
        }

        protected static string BaseUrl
        {
            get
            {
                var result = ConfigurationManager.AppSettings["ServerUrl"].ToString();
                if (result.StartsWith("http"))
                    return result;
                return "http://" + result;
            }
        }

        protected static void Login(string username, string password)
        {

            ApiCall("/sessions", new Dictionary<string, string>
                {
                    {"username", username},
                    {"password", password}
                });
        }
        
        private static CookieContainer _container = new CookieContainer();

        protected static void ApiCall(string url)
        {
            ApiCall(url, HttpMethod.Post);
        }

        protected static void ApiCall(string url, HttpMethod method)
        {
            ApiCall(url, method, new Dictionary<string, string>());
        }

        protected static void ApiCall(string url, IEnumerable<KeyValuePair<string, string>> formValues)
        {
            ApiCall(url, HttpMethod.Post,  formValues);
        }

        protected static void ApiCall(string url, HttpMethod method, IEnumerable<KeyValuePair<string, string>> formValues)
        {
            //var cookies = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = _container;
            var client = new HttpClient(handler);
            url = BaseUrl + "/api/" + url;

            HttpResponseMessage response;

            if (method == HttpMethod.Delete)
                response = client.DeleteAsync(url).Result;
            else
                response = client.PostAsync(url, new FormUrlEncodedContent(formValues)).Result;

            response.EnsureSuccessStatusCode();

            foreach (var each in _container.GetCookies(new Uri(BaseUrl)).Cast<Cookie>())
            {
                var data = String.Format("{0}={1}; expires={2}", new object[]
                    {
                        each.Name,
                        each.Value,
                        DateTime.Now.AddMinutes(30).ToString("R")
                    });
                Browser.SetCookie(BaseUrl, data);
            }

        }

        //protected static void ApiCall(string url, IEnumerable<KeyValuePair<string, string>> formValues)
        //{
        //    Browser.Post(new Uri("http://" + BaseUrl + "/api" + url), formValues);
        //}

        private static void CopyCookies(CookieContainer source, CookieContainer target)
        {
            
        }

        //[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref uint pcchCookieData, int dwFlags, IntPtr lpReserved);
        //const int INTERNET_COOKIE_HTTPONLY = 0x00002000;

        //public static string GetGlobalCookies(string uri)
        //{
        //    uint datasize = 1024;
        //    StringBuilder cookieData = new StringBuilder((int)datasize);
        //    if (InternetGetCookieEx(uri, null, cookieData, ref datasize, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero)
        //        && cookieData.Length > 0)
        //    {
        //        return cookieData.ToString().Replace(';', ',');
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //protected static void ApiCall(string url, IEnumerable<KeyValuePair<string, string>> formValues)
        //{

        //    Browser.GoTo(BaseUrl + "/account/logon");
        //    var container0 = Browser.GetCookieContainerForUrl(new Uri("http://" + BaseUrl));
        //    var count0 = container0.Count;
        //    Browser.TextField(Find.ByLabelText("E-Mail-Adresse")).Value = "user@test.phundus.ch";
        //    Browser.TextField(Find.ByLabelText("Passwort")).Value = "1234";
        //    Browser.Button(Find.ByValue("Anmelden")).Click();
        //    var container1 = Browser.GetCookieContainerForUrl(new Uri("http://" +BaseUrl));
        //    var count1 = container1.Count;
        //    Browser.GoTo(BaseUrl);
        //    var container2 = Browser.GetCookieContainerForUrl(new Uri("http://" + BaseUrl));
        //    var count2 = container2.Count;

            

        //    var content = new FormUrlEncodedContent(formValues);

        //    //var cookies = new CookieContainer();
        //    //var sessionCookie = Browser.GetCookie("http://localhost/", "ASP.NET_SessionId");
        //    //var authCookie = Browser.GetCookie("http://localhost/", ".ASPXAUTH");
        //    //var uri = new Uri("http://" + BaseUrl);
        //    //if (sessionCookie != null)
        //    //    cookies.Add(uri, new Cookie("ASP.NET_SessionId", sessionCookie));
        //    //if (authCookie != null)
        //    //    cookies.Add(uri, new Cookie("ASPXAUTH", authCookie));

        //    var handler = new HttpClientHandler();
        //    handler.Credentials = new NetworkCredential("mail@indr.ch", "1234");
        //    //handler.CookieContainer = cookies; // Browser.GetCookieContainerForUrl(new Uri("http://" + BaseUrl));
        //    //handler.CookieContainer = Browser.GetCookieContainerForUrl(new Uri("http://" + BaseUrl));
        //    Console.WriteLine("Cookies: " + handler.CookieContainer.Count);
        //    var client = new HttpClient(handler);
        //    var globalCookies = GetGlobalCookies(new Uri("http://" + BaseUrl).AbsoluteUri);
        //    if (globalCookies != null)
        //        handler.CookieContainer.SetCookies(new Uri("http://" + BaseUrl), globalCookies);
    

        //    var response = client.PostAsync("http://" + BaseUrl + "/api/" + url, content).Result;
        //    var setCookies = response.Headers.Where(p => p.Key == "Set-Cookie");
        //    response.EnsureSuccessStatusCode();
        //    response = client.PostAsync("http://" + BaseUrl + "/api/basket/clear", new FormUrlEncodedContent(new Dictionary<string, string>())).Result;
        //    response.EnsureSuccessStatusCode();
        //    Console.WriteLine("Cookies: " + handler.CookieContainer.Count);

        //    //foreach (var each in cookies.GetCookies(new Uri("http://" + BaseUrl)).Cast<Cookie>())
        //    //{
        //    //    var data = String.Format("{0}={1}; expires={2}", new object[]
        //    //        {
        //    //            each.Name,
        //    //            each.Value,
        //    //            DateTime.Now.AddMinutes(30).ToString("R")
        //    //        });
        //    //    Browser.SetCookie("http://" + BaseUrl, data);
        //    //}

            




        //    //var browserCookies = Browser.GetCookieContainerForUrl(new Uri("http://" + BaseUrl));

        //    //foreach (var each in browserCookies.GetCookies(new Uri("http://" + BaseUrl)).Cast<Cookie>())
        //    //{
        //    //    var data = String.Format("{0}={1}; expires={2}", new object[]
        //    //        {
        //    //            each.Name,
        //    //            each.Value,
        //    //            DateTime.Now.AddMinutes(30).ToString("R")
        //    //        });
        //    //    cookies.Add(each);
        //    //}
        //}
    }
}