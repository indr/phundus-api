namespace piNuts.phundus.Specs.Browsers
{
    using System.Configuration;
    using WatiN.Core;

    public class IeBrowser : IE
    {
        public string BaseUrl;

        public IeBrowser()
        {
            AutoClose = true;
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
    }
}