namespace Phundus.Common.Domain.Model
{
    using System;

    public class MembershipApplicationId : GuidIdentity
    {
        public MembershipApplicationId()
        {
        }

        public MembershipApplicationId(Guid id) : base(id)
        {
        }
    }

    public class ArticleShortId : Identity<int>
    {
        public ArticleShortId(int id) : base(id)
        {
        }

        protected ArticleShortId()
        {
        }
    }

    public class ArticleId : GuidIdentity
    {
        public ArticleId()
        {
        }

        public ArticleId(Guid guid) : base(guid)
        {
        }
    }

    public class CartId : GuidIdentity
    {
    }

    public class CartItemId : GuidIdentity
    {
        public CartItemId()
        {
        }

        public CartItemId(Guid id) : base(id)
        {
        }
    }

    public class CurrentUserId : InitiatorId
    {
        public CurrentUserId()
        {
        }

        public CurrentUserId(Guid id) : base(id)
        {
        }
    }

    public class InitiatorId : UserId
    {
        public InitiatorId()
        {
        }

        public InitiatorId(Guid id) : base(id)
        {
        }

        public InitiatorId(UserId userId) : base(userId.Id)
        {
        }
    }

    public class LesseeId : GuidIdentity
    {
        public LesseeId()
        {
        }

        public LesseeId(Guid id) : base(id)
        {
        }

        public LesseeId(UserId userId) : base(userId.Id)
        {
        }
    }

    public class LessorId : GuidIdentity
    {
        public LessorId()
        {
        }

        public LessorId(Guid id) : base(id)
        {
        }
    }

    public class OrderShortId : Identity<int>
    {
        public OrderShortId(int id) : base(id)
        {
        }

        protected OrderShortId()
        {
        }
    }

    public class OrderId : GuidIdentity
    {
        public OrderId()
        {
        }

        public OrderId(Guid id) : base(id)
        {
        }
    }

    public class OrderItemId : GuidIdentity
    {
        public OrderItemId()
        {
        }

        public OrderItemId(Guid id) : base(id)
        {
        }
    }

    public class OrganizationId : GuidIdentity
    {
        public OrganizationId()
        {
        }

        public OrganizationId(Guid id)
            : base(id)
        {
        }
    }

    public class OwnerId : GuidIdentity
    {
        public OwnerId()
        {
        }

        public OwnerId(Guid id)
            : base(id)
        {
        }
    }

    public class StoreId : GuidIdentity
    {
        public StoreId()
        {
        }

        public StoreId(Guid value)
            : base(value)
        {
        }
    }

    public class UserId : GuidIdentity
    {
        public UserId()
        {
        }

        public UserId(Guid id) : base(id)
        {
        }
    }
}