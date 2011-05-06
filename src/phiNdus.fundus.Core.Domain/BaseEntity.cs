using System;

namespace phiNdus.fundus.Core.Domain
{
    public class BaseEntity
    {
        public virtual int Id { get; protected set; }
        public virtual int Version { get; protected set; }
    }
}