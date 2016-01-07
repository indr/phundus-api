namespace Phundus.Specs.Browsers
{
    using System.Configuration;
    using WatiN.Core;

    public class IeBrowser : IE
    {
        public string BaseUrl;

        public IeBrowser()
        {
            AutoClose = ConfigurationManager.AppSettings["Unattended"] == "true";
            Settings.AutoMoveMousePointerToTopLeft = false;
            Settings.WaitForCompleteTimeOut = 60;

            ClearCache();
            ClearCookies();

            BaseUrl = ConfigurationManager.AppSettings["ServerUrl"];
        }

        public void GoToPage(string url)
        {
            GoTo(BaseUrl + url);
        }
    }
}