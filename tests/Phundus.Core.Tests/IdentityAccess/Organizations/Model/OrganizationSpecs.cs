namespace Phundus.Tests.IdentityAccess.Organizations.Model
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Model;
    using Rhino.Mocks;

    [Subject(typeof (Organization))]
    public class when_changing_startpage : aggregate_concern<Organization>
    {
        private static string theNewStartpage = "<p>The new startpage</p>";
        private static UserGuid theInitiatorId = new UserGuid();

        private Establish ctx = () => sut = new Organization(Guid.NewGuid(), "The organization");

        private Because of = () => sut.ChangeStartpage(theInitiatorId, theNewStartpage);

        private It should_have_new_startpage = () => sut.Startpage.ShouldEqual(theNewStartpage);

        private It should_publish_startpage_changed =
            () => publisher.AssertWasCalled(x => x.Publish(Arg<StartpageChanged>.Is.NotNull));
    }
}