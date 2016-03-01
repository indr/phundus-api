namespace Phundus.Common.Domain.Model
{
    using System;
    using System.ComponentModel;

    [TypeConverter(typeof (GuidConverter<ArticleId>))]
    public class ArticleId : GuidIdentity
    {
        public ArticleId()
        {
        }

        public ArticleId(Guid id) : base(id)
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

    [TypeConverter(typeof(GuidConverter<CartId>))]
    public class CartId : GuidIdentity
    {
    }

    [TypeConverter(typeof(GuidConverter<CartItemId>))]
    public class CartItemId : GuidIdentity
    {
        public CartItemId()
        {
        }

        public CartItemId(Guid id) : base(id)
        {
        }
    }

    [TypeConverter(typeof(GuidConverter<CurrentUserId>))]
    public class CurrentUserId : InitiatorId
    {
        public CurrentUserId()
        {
        }

        public CurrentUserId(Guid id) : base(id)
        {
        }
    }

    [TypeConverter(typeof(GuidConverter<InitiatorId>))]
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

    [TypeConverter(typeof(GuidConverter<LesseeId>))]
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

    [TypeConverter(typeof(GuidConverter<LessorId>))]
    public class LessorId : GuidIdentity
    {
        public LessorId()
        {
        }

        public LessorId(Guid id) : base(id)
        {
        }
    }

    [TypeConverter(typeof(GuidConverter<MembershipApplicationId>))]
    public class MembershipApplicationId : GuidIdentity
    {
        public MembershipApplicationId()
        {
        }

        public MembershipApplicationId(Guid id) : base(id)
        {
        }
    }

    [TypeConverter(typeof(GuidConverter<OrderId>))]
    public class OrderId : GuidIdentity
    {
        public OrderId()
        {
        }

        public OrderId(Guid id) : base(id)
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

    [TypeConverter(typeof(GuidConverter<OrderLineId>))]
    public class OrderLineId : GuidIdentity
    {
        public OrderLineId()
        {
        }

        public OrderLineId(Guid id) : base(id)
        {
        }
    }

    [TypeConverter(typeof(GuidConverter<OrganizationId>))]
    public class OrganizationId : GuidIdentity
    {
        public OrganizationId()
        {
        }

        public OrganizationId(Guid id) : base(id)
        {
        }
    }

    [TypeConverter(typeof(GuidConverter<OwnerId>))]
    public class OwnerId : GuidIdentity
    {
        public OwnerId()
        {
        }

        public OwnerId(Guid id) : base(id)
        {
        }
    }

    [TypeConverter(typeof(GuidConverter<StoreId>))]
    public class StoreId : GuidIdentity
    {
        public StoreId()
        {
        }

        public StoreId(Guid value) : base(value)
        {
        }
    }

    [TypeConverter(typeof(GuidConverter<UserId>))]
    public class UserId : GuidIdentity
    {
        public UserId()
        {
        }

        public UserId(Guid id) : base(id)
        {
        }
    }

    public class UserShortId : Identity<int>
    {
        public UserShortId(int id) : base(id)
        {
        }

        protected UserShortId()
        {
        }
    }
}