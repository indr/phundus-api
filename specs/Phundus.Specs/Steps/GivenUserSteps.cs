﻿namespace Phundus.Specs.Steps
{
    using Services;
    using Services.Entities;
    using TechTalk.SpecFlow;

    [Binding]
    public class GivenUserSteps : AppStepsBase
    {
        public GivenUserSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"a confirmed user")]
        public void AConfirmedUser()
        {
            AConfirmedUser(null);
        }

        [Given(@"a confirmed user ([^@""]*)")]
        [Given(@"a confirmed user ""([^@]*)""")]
        public void AConfirmedUser(string userKey)
        {
            AConfirmedUser(userKey, null);
        }

        [Given(@"a confirmed user ""([^@]*)"" with email address ""(.*)""")]
        public void AConfirmedUser(string userKey, string emailKey)
        {
            User user;
            if ((userKey != null) && Ctx.Users.TryGetValue(userKey, out user))
            {
                Ctx.User = user;
                return;
            }

            string emailAddress = null;
            if (emailKey != null)
                Ctx.EmailAddresses.TryGetValue(emailKey, out emailAddress);
            user = App.SignUpUser(emailAddress);
            App.ConfirmUser(user.UserId);

            Ctx.User = user;
            if (userKey != null)
                Ctx.Users[userKey] = user;
            if (emailKey != null)
                Ctx.EmailAddresses[emailKey] = user.EmailAddress;
        }
    }
}