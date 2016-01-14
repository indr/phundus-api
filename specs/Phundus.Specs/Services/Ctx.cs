namespace Phundus.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using ContentTypes;
    using Entities;
    using TechTalk.SpecFlow;

    public class Aliases<T>
    {
        private readonly IDictionary<string, T> _collection = new Dictionary<string, T>();

        public T this[string alias]
        {
            get
            {
                if (alias == null)
                    return default(T);
                T result;
                if (!_collection.TryGetValue(alias, out result))
                    throw new InvalidOperationException(String.Format("Alias {0} unknown.", alias));
                return result;
            }
            set
            {
                if (alias != "")
                    Debug.WriteLine("Aliased {0} to {1}.", alias, value);
                _collection[alias] = value;
            }
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public bool ContainsAlias(string alias)
        {
            return _collection.ContainsKey(alias);
        }

        public bool TryGetValue(string alias, out T value)
        {
            return _collection.TryGetValue(alias, out value);
        }
    }

    [Binding]
    public class Ctx
    {
        private readonly Aliases<Article> _articles = new Aliases<Article>();
        private readonly Aliases<string> _emailAddresses = new Aliases<string>();
        private readonly Aliases<Organization> _organizations = new Aliases<Organization>();
        private readonly Aliases<User> _users = new Aliases<User>();

        public User User
        {
            get { return _users[""]; }
            set { _users[""] = value; }
        }

        public Aliases<User> Users
        {
            get { return _users; }
        }

        public string EmailAddress
        {
            get { return _emailAddresses[""]; }
            set { _emailAddresses[""] = value; }
        }

        public Aliases<string> EmailAddresses
        {
            get { return _emailAddresses; }
        }

        public Organization Organization
        {
            get { return _organizations[""]; }
            set { _organizations[""] = value; }
        }

        public Aliases<Organization> Organizations
        {
            get { return _organizations; }
        }


        public Article Article
        {
            get { return _articles[""]; }
            set { _articles[""] = value; }
        }

        public Aliases<Article> Articles
        {
            get { return _articles; }
        }

        public Guid LoggedIn { get; set; }

        public string ValidationKey { get; set; }
        public Guid? Store { get; set; }
        public Order Order { get; set; }


        [BeforeScenario]
        public void BeforeScenario()
        {
            Users.Clear();
            EmailAddresses.Clear();
            Organizations.Clear();
            Articles.Clear();
            
            LoggedIn = Guid.Empty;
            ValidationKey = null;
            Store = null;
        }
    }
}