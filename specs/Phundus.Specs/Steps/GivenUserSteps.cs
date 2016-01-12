namespace Phundus.Specs.Steps
{
    using Services;
    using TechTalk.SpecFlow;

    [Binding]
    public class GivenUserSteps : StepsBase
    {
        public GivenUserSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"a confirmed user")]
        public void AConfirmedUser()
        {
            AConfirmedUser(null);
        }

        [Given(@"a confirmed user ""([^@]*)""")]
        public void AConfirmedUser(string userKey)
        {
            AConfirmedUser(userKey, null);
        }

        [Given(@"a confirmed user ""([^@]*)"" with email address ""(.*)""")]
        public void AConfirmedUser(string userKey, string emailKey)
        {
            string emailAddress = null;
            if (emailKey != null)
                Ctx.EmailAddresses.TryGetValue(emailKey, out emailAddress);
            var user = App.SignUpUser(emailAddress);
            App.ConfirmUser(user.Guid);

            Ctx.User = user;
            if (userKey != null)
                Ctx.Users[userKey] = user;
            if (emailKey != null)
                Ctx.EmailAddresses[emailKey] = user.EmailAddress;
        }
    }
}