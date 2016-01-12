﻿namespace Phundus.Specs.Services
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

        public void TryGetValue(string alias, out T value)
        {
            _collection.TryGetValue(alias, out value);
        }
    }

    [Binding]
    public class Ctx
    {
        private readonly Aliases<string> _emails = new Aliases<string>();
        private readonly Aliases<Organization> _organizations = new Aliases<Organization>();
        private readonly Aliases<User> _users = new Aliases<User>();

        public User User { get; set; }

        public Aliases<User> Users
        {
            get { return _users; }
        }

        public Aliases<string> Emails
        {
            get { return _emails; }
        }

        public Aliases<Organization> Organizations
        {
            get { return _organizations; }
        }

        public Organization Organization
        {
            get { return _organizations[""]; }
            set { _organizations[""] = value; }
        }

        /// <summary>
        /// Email address used by anonymous user
        /// </summary>
        public string AnonEmailAddress { get; set; }

        public Guid LoggedIn { get; set; }

        public string ValidationKey { get; set; }
        public Guid? Store { get; set; }

        [BeforeScenario]
        public void BeforeScenario()
        {
            User = null;
            Users.Clear();
            Organization = null;
            AnonEmailAddress = null;
            LoggedIn = Guid.Empty;
            ValidationKey = null;
            Emails.Clear();
            Organizations.Clear();
            Store = null;
        }
    }
}