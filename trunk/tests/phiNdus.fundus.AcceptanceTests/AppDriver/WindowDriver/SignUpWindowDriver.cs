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

        public void SpecifyStreet(string street)
        {
            Context.Browser.TextField(Find.ById("Street")).TypeText(street);
        }

        public void SpecifyPostcode(string postcode)
        {
            Context.Browser.TextField(Find.ById("Postcode")).TypeText(postcode);
        }

        public void SpecifyCity(string city)
        {
            Context.Browser.TextField(Find.ById("City")).TypeText(city);
        }

        public void SpecifyMobilePhone(string mobilePhone)
        {
            Context.Browser.TextField(Find.ById("MobilePhone")).TypeText(mobilePhone);
        }

        public void SpecifyJsNumber(string jsNumber)
        {
            Context.Browser.TextField(Find.ById("JsNumber")).TypeText(jsNumber);
        }

        public void SpecifyOrganization(string organization)
        {
            Context.Browser.SelectList(Find.ById("OrganizationId")).Select(organization);
        }

        public void SpecifyPassword(string password)
        {
            Context.Browser.TextField(Find.ById("Password")).TypeText(password);
        }

        public void SpecifyAll(string email, string password = "password", string firstName = "Dave",
            string lastName = "Miller", string jsNumber = "100000")
        {
            SpecifyEmail(email);
            SpecifyPassword(password);
            SpecifyPasswordAgain(password);
            
            SpecifyFirstName(firstName);
            SpecifyLastName(lastName);
            SpecifyStreet("Strasse");
            SpecifyPostcode("6003");
            SpecifyCity("Luzern");
            SpecifyMobilePhone("0788138580");

            SpecifyJsNumber(jsNumber);
            SpecifyOrganization("Pfadi Lego");
        }

        private void SpecifyPasswordAgain(string password)
        {
            Context.Browser.TextField(Find.ById("PasswordAgain")).TypeText(password);
        }
    }
}