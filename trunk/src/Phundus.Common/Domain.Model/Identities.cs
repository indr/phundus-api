namespace Phundus.Common.Domain.Model
{
    using System;

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
    }

    public class OwnerId : Identity<Guid>
    {
        public OwnerId(Guid id)
            : base(id)
        {
        }

        protected OwnerId()
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
}