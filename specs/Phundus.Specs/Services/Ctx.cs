namespace Phundus.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using TechTalk.SpecFlow;

    [Binding]
    public class Ctx
    {
        private readonly IDictionary<string, User> _users = new Dictionary<string, User>();
        private readonly IDictionary<string, string> _emails = new Dictionary<string, string>();

        public User User { get; set; }

        public IDictionary<string, User> Users
        {
            get { return _users; }
        }

        public Organization Organization { get; set; }

        /// <summary>
        /// Email address used by anonymous user
        /// </summary>
        public string AnonEmailAddress { get; set; }

        public Guid LoggedIn { get; set; }
        public string ValidationKey { get; set; }

        public IDictionary<string, string> Emails
        {
            get { return _emails; }
        }
    }
}