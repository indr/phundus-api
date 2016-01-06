namespace phiNdus.fundus.Web.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.Security;
    using Castle.Transactions;
    using Phundus.Core.IdentityAndAccess.Users.Repositories;

    public class CustomRoleProvider : RoleProvider
    {
        public IUserRepository Users { get; set; }

        public override string ApplicationName { get; set; }

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

        [Transaction]
        public override string[] GetRolesForUser(string username)
        {
            var user = Users.FindByEmailAddress(username);
            var role = user.Role.ToString();

            var result = new List<string>();

            if (role == @"Admin")
                result.Add(@"Admin");

            if ((role == @"Admin") || (username.EndsWith("@test.phundus.ch")))
                result.Add(@"Beta");

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