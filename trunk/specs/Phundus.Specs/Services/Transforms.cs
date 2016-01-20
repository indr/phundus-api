namespace Phundus.Specs.Services
{
    using TechTalk.SpecFlow;

    [Binding]
    public class Transforms
    {
        [StepArgumentTransformation(@"(on|off)")]
        public bool OnOffTransform(string value)
        {
            return value.ToLowerInvariant() == @"on";
        }
    }
}