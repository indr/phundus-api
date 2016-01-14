namespace Phundus.Common.Domain.Model
{
    using System;

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
        public CartGuid() : base(Guid.NewGuid())
        {
            
        }
    }

    public class CartItemId : Identity<int>
    {
    }

    public class CartItemGuid : GuidIdentity
    {
        public CartItemGuid(Guid id) : base(id)
        {
        }

        public CartItemGuid() : base(Guid.NewGuid())
        {
        }
    }

    public class CurrentUserId : UserId
    {
        public CurrentUserId(int id) : base(id)
        {
        }
    }

    public class CurrentUserGuid : UserGuid
    {
        public CurrentUserGuid(Guid id) : base(id)
        {
        }
    }

    public class LesseeId : Identity<int>
    {
        public LesseeId(int id) : base(id)
        {
        }
    }

    public class LessorId : GuidIdentity
    {
        public LessorId(Guid id) : base(id)
        {
        }

        public LessorId() : base(Guid.NewGuid())
        {
        }
    }

    public class OwnerId : GuidIdentity
    {
        public OwnerId(Guid id)
            : base(id)
        {
        }

        public OwnerId() : base(Guid.NewGuid())
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

    public class StoreId : GuidIdentity
    {
        public StoreId()
            : base(Guid.NewGuid())
        {
        }

        public StoreId(Guid value)
            : base(value)
        {
        }
    }

    public class UserId : Identity<int>
    {
        public UserId(int id) : base(id)
        {
        }

        protected UserId()
        {
        }
    }

    public class UserGuid : GuidIdentity
    {
        public UserGuid() : base(Guid.NewGuid())
        {
        }

        public UserGuid(Guid id) : base(id)
        {
        }
    }
}