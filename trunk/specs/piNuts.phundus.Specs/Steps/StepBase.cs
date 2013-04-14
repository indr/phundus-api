namespace piNuts.phundus.Specs.Steps
{
    using System.Configuration;
    using Browsers;

    public class StepBase
    {
        protected static IeBrowser Browser
        {
            get { return Browsers.Browser.Current; }
        }

        protected static string BaseUrl
        {
            get { return ConfigurationManager.AppSettings["ServerUrl"]; }
        }
    }
}