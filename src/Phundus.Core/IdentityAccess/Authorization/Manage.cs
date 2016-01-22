namespace Phundus.IdentityAccess.Authorization
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Phundus.Authorization;
    using Queries;

    public static class Manage
    {
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
}