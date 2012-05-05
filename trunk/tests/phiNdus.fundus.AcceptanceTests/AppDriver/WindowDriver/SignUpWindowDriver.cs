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
            Context.Browser.TextField(Find.ById("Email")).TypeText(email);
        }

        public void SignUp()
        {
            Context.Browser.Button(Find.ByValue("Registrieren")).Click();
        }

        public void SpecifyFirstName(string firstName)
        {
            Context.Browser.TextField(Find.ById("FirstName")).TypeText(firstName);
        }

        public void SpecifyLastName(string lastName)
        {
            Context.Browser.TextField(Find.ById("LastName")).TypeText(lastName);
        }

        public void SpecifyJsNumber(string jsNumber)
        {
            Context.Browser.TextField(Find.ById("JsNumber")).TypeText(jsNumber);
        }

        public void SpecifyPassword(string password)
        {
            Context.Browser.TextField(Find.ById("Password")).TypeText(password);
        }

        public void SpecifyAll(string email, string password = "password", string firstName = "Dave",
            string lastName = "Miller", string jsNumber = "1")
        {
            SpecifyFirstName(firstName);
            SpecifyLastName(lastName);
            SpecifyEmail(email);
            SpecifyJsNumber(jsNumber);
            SpecifyPassword(password);
            
        }
    }
}