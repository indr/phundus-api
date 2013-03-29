namespace piNuts.phundus.Specs
{
    using TechTalk.SpecFlow;
    using piNuts.phundus.Specs.Browsers;

    /// <summary>
    /// http://volaresystems.com/Blog/post/2013/01/06/SpecFlow-and-WatiN-Worst-Practices-What-NOT-to-do.aspx
    /// </summary>
    [Binding]
    public class BeforeAndAfter
    {
        [AfterScenario]
        public static void AfterTestRun()
        {
            if (!ScenarioContext.Current.ContainsKey("browser"))
                return;
            var browser = ScenarioContext.Current["browser"] as IeBrowser;
            if (browser != null) browser.Close();
        }
    }
}