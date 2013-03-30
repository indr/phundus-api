namespace piNuts.phundus.Specs.Steps
{
    using System.Configuration;
    using TechTalk.SpecFlow;
    using piNuts.phundus.Specs.Browsers;

    public class StepBase
    {
        protected static IeBrowser Browser
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("browser"))
                    ScenarioContext.Current["browser"] = new IeBrowser();
                return ScenarioContext.Current["browser"] as IeBrowser;
            }
        }

        protected static string BaseUrl
        {
            get { return ConfigurationManager.AppSettings["ServerUrl"]; }
        }
    }
}