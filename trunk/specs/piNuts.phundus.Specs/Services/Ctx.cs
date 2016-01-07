namespace Phundus.Specs.Services
{
    using System;
    using TechTalk.SpecFlow;

    [Binding]
    public class Ctx
    {
        public Guid User { get; set; }
    }
}