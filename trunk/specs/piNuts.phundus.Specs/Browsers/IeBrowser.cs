namespace piNuts.phundus.Specs.Browsers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net.Http;
    using System.Text;
    using SHDocVw;
    using WatiN.Core;

    public class IeBrowser : IE
    {
        public string BaseUrl;

        public IeBrowser()
        {
            AutoClose = ConfigurationManager.AppSettings["Unattended"] == "true";
            Settings.AutoMoveMousePointerToTopLeft = false;

            ClearCache();
            ClearCookies();

            BaseUrl = ConfigurationManager.AppSettings["ServerUrl"];
        }

        //public T GoToPage<T>() where T : PageBase<T>, new()
        //{
        //    GoTo(BaseUrl);

        //    return Page<T>();
        //}
        public void GoToPage(string url)
        {
            GoTo(BaseUrl + url);
        }
        
        public void Post(Uri baseUri, IEnumerable<KeyValuePair<string, string>> postData)
        {
            object flags = null;
            object targetFrame = null;
            
            //object postDataBytes = MakeByteStreamOf(postData);
            var content = new FormUrlEncodedContent(postData).ReadAsStringAsync().Result;
            object postDataBytes = Encoding.UTF8.GetBytes(content);

            object headers = "Content-Type: application/x-www-form-urlencoded" + Convert.ToChar(13) + Convert.ToChar(10)
                + "Content-Length: " + (postDataBytes as byte[]).Length + Convert.ToChar(13) + Convert.ToChar(10);


            object resourceLocator = baseUri.ToString();
            var browser = (IWebBrowser2) this.InternetExplorer;
            browser.Navigate2(ref resourceLocator, 0x0200, ref targetFrame, ref postDataBytes, ref headers);
            this.WaitForComplete();
        }

        private static byte[] MakeByteStreamOf(IEnumerable<KeyValuePair<string, string>> postData)
        {
            var sb = new StringBuilder();
           
                foreach (var postDataEntry in postData)
                {
                    sb.Append(postDataEntry.Key).Append('=').Append(postDataEntry.Value).Append('&');
                }
                sb.Remove(sb.Length - 1, 1);
            
            return Encoding.ASCII.GetBytes(sb.ToString());
        }
    }


}

