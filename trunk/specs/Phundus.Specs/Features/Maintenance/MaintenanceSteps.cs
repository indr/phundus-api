using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phundus.Specs.Features.Maintenance
{
    using ContentTypes;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class MaintenanceSteps : AppStepsBase
    {
        public MaintenanceSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [AfterFeature]
        public static void DeactivateMaintenanceMode()
        {
            if (App.InMaintenanceMode)
            {
                new Resource("sessions", false).Post(new SessionsPostRequestContent
                {
                    Username = "admin@test.phundus.ch",
                    Password = "1234"
                });
                new Resource("maintenance", false).Patch(new {inMaintenance = false});
                App.InMaintenanceMode = false;
            }
        }

        [Given("in maintenance mode")]
        public void ActivateMaintenanceMode()
        {
            if (!App.InMaintenanceMode)
            {
                new Resource("sessions", false).Post(new SessionsPostRequestContent
                {
                    Username = "admin@test.phundus.ch",
                    Password = "1234"
                });
                new Resource("maintenance", false).Patch(new { inMaintenance = true });
                App.InMaintenanceMode = true;
            }
        }
        
        [Given(@"I activated maintenance mode")]
        public void GivenIActivatedMaintenanceMode()
        {
            App.SetMaintenanceMode(true);
        }

        [When(@"I try to activate maintenance mode")]
        public void WhenITryToActivateMaintenanceMode()
        {
            App.SetMaintenanceMode(true, false);
        }

        [When(@"I try to deactivate maintenance mode")]
        public void WhenITryToDeactivateMaintenanceMode()
        {
            App.SetMaintenanceMode(false, false);
        }
    }
}
