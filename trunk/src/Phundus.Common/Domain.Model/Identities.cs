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

    public class CurrentUserId : Identity<int>
    {
        public CurrentUserId(int id) : base(id)
        {
        }
    }

    public class CurrentUserGuid : InitiatorGuid
    {
        public CurrentUserGuid()
        {
        }

        public CurrentUserGuid(Guid id) : base(id)
        {
        }
    }

    public class InitiatorGuid : UserGuid
    {
        public InitiatorGuid()
        {
        }

        public InitiatorGuid(Guid id) : base(id)
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

    public class UserGuid : GuidIdentity
    {
        public UserGuid()
        {
        }

        public UserGuid(Guid id) : base(id)
        {
        }
    }
}