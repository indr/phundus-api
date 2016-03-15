namespace Phundus.Specs.Steps
{    
    using System.Diagnostics;
    using Services;
    using TechTalk.SpecFlow;

    [Binding]
    public class LogInSteps : AppStepsBase
    {
        public LogInSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"I logged in as root")]
        [Given(@"I am logged in as root")]
        public void GivenIAmLoggedInAsRoot()
        {
            App.LogInAsRoot();
        }        

        [Given(@"I am logged in as a user")]
        public void GivenIAmLoggedInAsAUser()
        {
            Given("a confirmed user");
            var user = Ctx.User;
            App.LogIn(user.Username, user.Password);
        }

        [Given(@"I am logged in as ""?((?!root)[^ ""]*)""?")]
        [When(@"I am logged in as ""?((?!root)[^ ""]*)""?")]
        public void GivenIAmLoggedInAs(string alias)
        {
            if (!Ctx.Users.ContainsAlias(alias))
                Given(@"a confirmed user """ + alias + @"""");
            var user = Ctx.Users[alias];
            Debug.WriteLine("Logging in as {0} {1}.", new [] {alias, user.ToString()});
            App.LogIn(user.Username, user.Password);
            Ctx.User = user;
        }

        [Given(@"I am logged out")]
        public void GivenIAmLoggedOut()
        {
            App.LogOut();
        }


        [When(@"I try to log in as (.*)")]
        public void WhenITryToLogInAs(string alias)
        {
            var user = Ctx.Users[alias];
            App.LogIn(user.Username, user.Password, false);
        }

        [When(@"I try to log in with ""(.*)""")]
        public void WhenITryToLogInWith(string username)
        {
            App.LogIn(username, "password", false);
        }
    }
}