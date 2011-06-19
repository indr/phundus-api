using NUnit.Framework;

namespace phiNdus.fundus.AcceptanceTests.AppDriver.WindowDriver
{
    internal class AbstractWindowDriver
    {
        public AbstractWindowDriver(TestContext context)
        {
            Context = context;
        }

        protected readonly TestContext Context;

        public void ContainsText(string text)
        {
            Assert.IsTrue(Context.Browser.ContainsText(text), string.Format(@"Text not found: {0}", text));
        }
    }
}