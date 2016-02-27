namespace Phundus.Tests.IdentityAccess.Commands
{
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Organizations.Model;

    [Subject(typeof (ChangeOrganizationPlanHandler))]
    public class when_handling_change_organization_plan :
        identityaccess_command_handler_concern<ChangeOrganizationPlan, ChangeOrganizationPlanHandler>
    {
        private static Organization theOrganization;

        private Establish ctx = () =>
        {
            theOrganization = make.Organization();
            organizationRepository.WhenToldTo(x => x.GetById(theOrganization.Id)).Return(theOrganization);

            command = new ChangeOrganizationPlan(theInitiatorId, theOrganization.Id, "membership");
        };

        private It should_tell_organization_to_change_plan = () =>
            theOrganization.WasToldTo(x => x.ChangePlan(theAdmin, OrganizationPlan.Membership));
    }
}