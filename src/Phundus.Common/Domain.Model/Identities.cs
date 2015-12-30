namespace Phundus.Common.Domain.Model
{
    using System;

    public class UserId : Identity<int>
    {
        public UserId(int value) : base(value)
        {
        }

        protected UserId()
        {
        }
    }

    public class OwnerId : Identity<Guid>
    {
        public OwnerId(Guid value) : base(value)
        {
        }

        protected OwnerId()
        {
        }
    }
}