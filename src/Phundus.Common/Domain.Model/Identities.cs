namespace Phundus.Common.Domain.Model
{
    using System;

    public class ArticleId : Identity<int>
    {
        public ArticleId(int id) : base(id)
        {
        }
    }

    public class CurrentUserId : UserId
    {
        public CurrentUserId(int id) : base(id)
        {
        }
    }

    public class LesseeId : Identity<int>
    {
        public LesseeId(int id) : base(id)
        {
        }
    }

    public class LessorId : Identity<Guid>
    {
        public LessorId(Guid id) : base(id)
        {
        }

        public LessorId() : base(Guid.NewGuid())
        {
        }
    }

    public class OwnerId : Identity<Guid>
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

    public class UserId : Identity<int>
    {
        public UserId(int id) : base(id)
        {
        }

        protected UserId()
        {
        }
    }

    public class UserGuid : Identity<Guid>
    {
        public UserGuid(Guid id) : base(id)
        {
        }
    }
}