namespace Phundus.Tests.IdentityAccess.Model
{
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model;
    using Phundus.IdentityAccess.Organizations.Model;
    using Rhino.Mocks;

    [Subject(typeof (Settings))]
    public class when_instantiating_settings : Observes<Settings>
    {
        private It should_have_public_rental_false = () =>
            sut.PublicRental.ShouldBeFalse();
    }

    [Subject(typeof(Organization))]
    public class when_changing_setting_public_rental_to_true : organization_concern
    {
        private Because of = () =>
            sut.ChangeSettingPublicRental(theInitiator, true);

        private It should_have_setting_public_rental_true = () =>
            sut.Settings.PublicRental.ShouldBeTrue();

        private It should_publish_public_rental_setting_changed = () =>
            publisher.WasToldTo(x => x.Publish(Arg<PublicRentalSettingChanged>.Is.NotNull));
    }
}