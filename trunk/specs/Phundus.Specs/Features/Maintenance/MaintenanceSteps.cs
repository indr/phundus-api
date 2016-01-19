using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phundus.Specs.Features.Maintenance
{
    using Services;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class MaintenanceSteps : AppStepsBase
    {
        public MaintenanceSteps(App app, Ctx ctx) : base(app, ctx)
        {
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
