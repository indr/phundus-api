namespace phiNdus.fundus.AcceptanceTests.AppDriver.WindowDriver
{
    internal class ValidationWindowDriver : AbstractWindowDriver
    {
        public ValidationWindowDriver(TestContext context) : base(context)
        {
            Context.Browser.GoTo(Context.BaseUri + "/Account/Validation");
        }

        public ValidationWindowDriver(TestContext context, string uri) : base(context)
        {
            Context.Browser.GoTo(uri);
        }
    }
}