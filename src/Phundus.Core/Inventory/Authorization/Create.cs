namespace Phundus.Inventory.Authorization
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Queries;
    using Phundus.Authorization;

    public static class Create
    {
        public static CreateArticleAccessObject Article(OwnerId ownerId)
        {
            return new CreateArticleAccessObject(ownerId);
        }
    }

    public class CreateArticleAccessObject : IAccessObject
    {
        public CreateArticleAccessObject(OwnerId ownerId)
        {
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            OwnerId = ownerId;
        }

        public OwnerId OwnerId { get; protected set; }
    }

    public class CreateArticleAccessObjectHandler : IHandleAccessObject<CreateArticleAccessObject>
    {
        private readonly IMemberInRole _memberInRole;

        public CreateArticleAccessObjectHandler(IMemberInRole memberInRole)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            _memberInRole = memberInRole;
        }

        public void Enforce(UserId userId, CreateArticleAccessObject accessObject)
        {
            if (!Test(userId, accessObject))
                throw new AuthorizationException("Du benötigst die Rolle Verwaltung.");
        }

        public bool Test(UserId userId, CreateArticleAccessObject accessObject)
        {
            return _memberInRole.IsActiveManager(accessObject.OwnerId, userId);
        }
    }
}