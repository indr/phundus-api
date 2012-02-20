using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Security {
    public class FundusRoleProvider : RoleProvider {

        //=========================================================================================
        #region Configuration

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config) {
            base.Initialize(name, config);

            ApplicationName = config["applicationName"];
        }
        #endregion
        //=========================================================================================

        public override void AddUsersToRoles(string[] usernames, string[] roleNames) {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }

        public override void CreateRole(string roleName) {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles() {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            return IoC.Resolve<IRoleService>().GetRolesForUser(HttpContext.Current.Session.SessionID);
        }

        public override string[] GetUsersInRole(string roleName) {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName) {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName) {
            throw new NotImplementedException();
        }
    }
}