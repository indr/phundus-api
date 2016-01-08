﻿namespace Phundus.Specs.Services
{
    using Entities;
    using TechTalk.SpecFlow;

    [Binding]
    public class Ctx
    {
        public User CurrentUser { get; set; }
        public Organization CurrentOrganization { get; set; }

        /// <summary>
        /// Email address used by anonymous user
        /// </summary>
        public string AnonEmailAddress { get; set; }
    }
}