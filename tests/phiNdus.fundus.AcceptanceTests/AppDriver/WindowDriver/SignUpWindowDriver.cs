using System;
using WatiN.Core;

namespace phiNdus.fundus.AcceptanceTests.AppDriver.WindowDriver
{
    internal class SignUpWindowDriver : AbstractWindowDriver
    {
        public SignUpWindowDriver(TestContext context) : base(context)
        {
            
            Context.Browser.GoTo(Context.BaseUri + "/Account/SignUp");
        }

        public void SpecifyEmail(string email)
        {
            Context.Browser.TextField(Find.ById("Email")).TypeText("abc.com");
        }

        public void SignUp()
        {
            Context.Browser.Button(Find.ByValue("Registrieren")).Click();
        }
    }
}