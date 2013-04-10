namespace phiNdus.fundus.Web.Security
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web.Security;
    using Castle.Windsor;

    public abstract class WindsorRoleProvider : RoleProvider
    {
        string _providerId;
        public override string ApplicationName { get; set; }

        public override bool IsUserInRole(string username, string roleName)
        {
            return WithProvider(p => p.IsUserInRole(username, roleName));
        }

        public override string[] GetRolesForUser(string username)
        {
            return WithProvider(p => p.GetRolesForUser(username));
        }

        public override void CreateRole(string roleName)
        {
            WithProvider(p => p.CreateRole(roleName));
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return WithProvider(p => p.DeleteRole(roleName, throwOnPopulatedRole));
        }

        public override bool RoleExists(string roleName)
        {
            return WithProvider(p => p.RoleExists(roleName));
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            WithProvider(p => p.AddUsersToRoles(usernames, roleNames));
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            WithProvider(p => p.RemoveUsersFromRoles(usernames, roleNames));
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return WithProvider(p => p.GetUsersInRole(roleName));
        }

        public override string[] GetAllRoles()
        {
            return WithProvider(p => p.GetAllRoles());
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return WithProvider(p => p.FindUsersInRole(roleName, usernameToMatch));
        }

        public abstract IWindsorContainer GetContainer();

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
            _providerId = config["providerId"];
            if (string.IsNullOrWhiteSpace(_providerId))
                throw new Exception("Please configure the providerId from the membership provider " + name);
        }

        RoleProvider GetProvider()
        {
            try
            {
                var provider = GetContainer().Resolve<RoleProvider>(_providerId, new Hashtable());
                if (provider == null)
                    throw new Exception(string.Format("Component '{0}' does not inherit RoleProvider", _providerId));
                return provider;
            }
            catch (Exception e)
            {
                throw new Exception("Error resolving RoleProvider " + _providerId, e);
            }
        }

        T WithProvider<T>(Func<RoleProvider, T> p)
        {
            var provider = GetProvider();
            try
            {
                return p(provider);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        void WithProvider(Action<RoleProvider> p)
        {
            var provider = GetProvider();
            try
            {
                p(provider);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }
    }
}