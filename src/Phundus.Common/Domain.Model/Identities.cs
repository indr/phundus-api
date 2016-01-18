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

    public class ArticleId : Identity<int>
    {
        public ArticleId(int id) : base(id)
        {
        }
    }

    public class CartId : Identity<int>
    {
        public CartId(int id) : base(id)
        {
        }
    }

    public class CartGuid : GuidIdentity
    {
    }

    public class CartItemId : Identity<int>
    {
    }

    public class CartItemGuid : GuidIdentity
    {
        public CartItemGuid()
        {
        }

        public CartItemGuid(Guid id) : base(id)
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
    }

    public class LesseeId : GuidIdentity
    {
        public LesseeId()
        {
        }

        public LesseeId(Guid id) : base(id)
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

    public class OrderId : Identity<int>
    {
        public OrderId(int id) : base(id)
        {
        }

        protected OrderId()
        {
        }
    }

    public class OrderItemId : GuidIdentity
    {
    }

    public class OrganizationGuid : GuidIdentity
    {
        public OrganizationGuid()
        {
        }

        public OrganizationGuid(Guid id)
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