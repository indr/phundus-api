namespace Phundus.Specs.Services
{
    using System;
    using System.IO;
    using ContentTypes;
    using Entities;
    using TechTalk.SpecFlow;

    [Binding]
    public class Ctx
    {
        private static readonly Aliases<Article> _articles = new Aliases<Article>();
        private static readonly Aliases<string> _emailAddresses = new Aliases<string>();
        private static readonly Aliases<Organization> _organizations = new Aliases<Organization>();
        private static readonly Aliases<Guid> _stores = new Aliases<Guid>(); 
        private static readonly Aliases<User> _users = new Aliases<User>();
        private static readonly Aliases<string> _fileNames = new Aliases<string>();

        public User User
        {
            get { return _users[""]; }
            set { _users[""] = value; }
        }

        public static Aliases<User> Users
        {
            get { return _users; }
        }

        public string EmailAddress
        {
            get { return _emailAddresses[""]; }
            set { _emailAddresses[""] = value; }
        }

        public static Aliases<string> EmailAddresses
        {
            get { return _emailAddresses; }
        }

        public static Aliases<string> FileNames
        {
            get { return _fileNames; }
        }

        public Organization Organization
        {
            get { return _organizations[""]; }
            set { _organizations[""] = value; }
        }

        public static Aliases<Organization> Organizations
        {
            get { return _organizations; }
        }

        public Article Article
        {
            get { return _articles[""]; }
            set { _articles[""] = value; }
        }

        public static Aliases<Article> Articles
        {
            get { return _articles; }
        }

        public static Aliases<Guid> Stores
        {
            get { return _stores; }
        }

        public static Guid LoggedIn { get; set; }

        public static string ValidationKey { get; set; }
        public Order Order { get; set; }

        [BeforeFeature]
        public static void BeforeScenario()
        {
            FileNames.Clear();
            Users.Clear();
            EmailAddresses.Clear();
            Organizations.Clear();
            Stores.Clear();
            Articles.Clear();

            LoggedIn = Guid.Empty;
            ValidationKey = null;            
        }
    }
}