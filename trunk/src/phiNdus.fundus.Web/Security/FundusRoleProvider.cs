namespace phiNdus.fundus.Web.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.Security;
    using Domain.Repositories;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class FundusRoleProvider : RoleProvider
    {
        public FundusRoleProvider()
        {
            UserRepositoryFactory = () => GlobalContainer.Resolve<IUserRepository>();
        }

        protected IUserRepository Users
        {
            get { return UserRepositoryFactory(); }
        }

        public override string ApplicationName { get; set; }

        public Func<IUserRepository> UserRepositoryFactory { get; set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            ApplicationName = config["applicationName"];
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotSupportedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotSupportedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotSupportedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotSupportedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var user = Users.FindByEmail(username);
            var role = user.Role.Name;

            var result = new List<string>();

            if (role == @"Admin")
                result.Add(@"Admin");

            if (user.IsChiefOf(user.SelectedOrganization))
                result.Add(@"Chief");

            result.Add(@"User");

            return result.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotSupportedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotSupportedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotSupportedException();
        }
    }
}