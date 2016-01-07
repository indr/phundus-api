namespace Phundus.Specs.Services
{
    using System.Net.Mail;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class Ctx
    {
        public User User { get; set; }
    }
}