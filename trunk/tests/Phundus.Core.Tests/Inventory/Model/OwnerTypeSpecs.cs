namespace Phundus.Tests.Inventory.Model

{
    using Machine.Specifications;
    using Phundus.Inventory.Model;

    /// <summary>
    /// This is important for domain event serialization!
    /// </summary>
    [Subject(typeof (OwnerType))]
    public class OwnerTypeSpecs
    {
        private It should_have_organization_equal_1 = () =>
            ((int) OwnerType.Organization).ShouldEqual(1);

        private It should_have_unknown_equal_0 = () =>
            ((int) OwnerType.Unknown).ShouldEqual(0);

        private It should_have_user_equal_2 = () =>
            ((int) OwnerType.User).ShouldEqual(2);
    }
}