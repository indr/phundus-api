namespace Phundus.Common.Domain.Model
{
    using System;

    public class UserId : Identity<int>
    {
        public UserId(int id) : base(id)
        {
        }

        protected UserId()
        {
        }
    }

    public class OwnerId : Identity<Guid>
    {
        public OwnerId(Guid id) : base(id)
        {
        }

        protected OwnerId()
        {
        }
    }
}