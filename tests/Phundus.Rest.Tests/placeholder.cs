namespace Phundus.Rest.Tests
{
    using Machine.Specifications;

    [Subject("placeholder")]
    public class placeholder
    {
        private It should_not_be_a_placeholder = () =>
            true.ShouldBeTrue();
    }
}