namespace Phundus.Inventory.Authorization
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Projections;
    using IdentityAccess.Resources;
    using Phundus.Authorization;

    public static class Manage
    {
        public static ManageArticlesAccessObject Articles(OwnerId ownerId)
        {
            return new ManageArticlesAccessObject(ownerId);
        }
    }

    public class ManageArticlesAccessObject : IAccessObject
    {
        public ManageArticlesAccessObject(OwnerId ownerId)
        {
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            OwnerId = ownerId;
        }

        public OwnerId OwnerId { get; protected set; }
    }

    public class ManageArticlesAccessObjectHandler : IHandleAccessObject<ManageArticlesAccessObject>
    {
        private readonly IMemberInRole _memberInRole;

        public ManageArticlesAccessObjectHandler(IMemberInRole memberInRole)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            _memberInRole = memberInRole;
        }

        public void Enforce(UserId userId, ManageArticlesAccessObject accessObject)
        {
            if (!Test(userId, accessObject))
                throw new AuthorizationException("Du benötigst die Rolle Verwaltung.");
        }

        public bool Test(UserId userId, ManageArticlesAccessObject accessObject)
        {
            return _memberInRole.IsActiveManager(accessObject.OwnerId, userId);
        }
    }
}