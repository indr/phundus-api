using Iesi.Collections.Generic;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class DomainObject : BasePropertyEntity
    {
        private ISet<DomainObject> _children = new HashedSet<DomainObject>();

        public virtual ISet<DomainObject> Children
        {
            get { return _children; }
            protected set { _children = value; }
        }

        public virtual DomainObject Parent { get; protected set; }

        public virtual void AddChild(DomainObject item)
        {
            _children.Add(item);
            item.Parent = this;
        }
    }
}