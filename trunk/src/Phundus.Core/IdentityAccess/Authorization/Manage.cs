namespace Phundus.IdentityAccess.Authorization
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Phundus.Authorization;
    using Projections;
    using Users.Services;

    public static class Manage
    {
        public static ManageUsersAccessObject Users
        {
            get { return new ManageUsersAccessObject(); }
        }

        public static ManageOrganizationAccessObject Organization(OrganizationId organizationId)
        {
            return new ManageOrganizationAccessObject(organizationId);
        }
    }

    public class ManageOrganizationAccessObject : IAccessObject
    {
        public ManageOrganizationAccessObject(OrganizationId organizationId)
        {
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            OrganizationId = organizationId;
        }

        public OrganizationId OrganizationId { get; protected set; }
    }

    public class ManageOrganizationAccessObjectHandler : IHandleAccessObject<ManageOrganizationAccessObject>
    {
        private readonly IMemberInRole _memberInRole;

        public ManageOrganizationAccessObjectHandler(IMemberInRole memberInRole)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            _memberInRole = memberInRole;
        }

        public void Enforce(UserId userId, ManageOrganizationAccessObject accessObject)
        {
            if (!Test(userId, accessObject))
                throw new AuthorizationException("Du benötigst die Rolle Verwaltung.");
        }

        public bool Test(UserId userId, ManageOrganizationAccessObject accessObject)
        {
            return _memberInRole.IsActiveManager(accessObject.OrganizationId, userId);
        }
    }

    public class ManageUsersAccessObject : IAccessObject
    {
    }

    public class ManageUsersAccessObjectHandler : IHandleAccessObject<ManageUsersAccessObject>
    {
        private readonly IUserInRole _userInRole;

        public ManageUsersAccessObjectHandler(IUserInRole userInRole)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            _userInRole = userInRole;
        }

        public void Enforce(UserId userId, ManageUsersAccessObject accessObject)
        {
            if (!Test(userId, accessObject))
                throw new AuthorizationException("Du benötigst die Rolle Administration.");
        }

        public bool Test(UserId userId, ManageUsersAccessObject accessObject)
        {
            return _userInRole.IsAdmin(userId);
        }
    }
}