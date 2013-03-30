namespace piNuts.phundus.Specs.Browsers
{
    public static class Browser
    {
        private static IeBrowser _instance;

        public static IeBrowser Current
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = new IeBrowser();
                return _instance;
            }
        }
    }
}