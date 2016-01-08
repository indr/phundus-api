namespace Phundus.Specs.Services
{
    using System;
    using Entities;
    using TechTalk.SpecFlow;

    [Binding]
    public class Ctx
    {
        public User User { get; set; }
        public Organization Organization { get; set; }

        /// <summary>
        /// Email address used by anonymous user
        /// </summary>
        public string AnonEmailAddress { get; set; }

        public Guid LoggedIn { get; set; }
    }
}