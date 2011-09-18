using System;
using Iesi.Collections.Generic;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class DomainObject : BasePropertyEntity
    {
        private ISet<DomainObject> _children = new HashedSet<DomainObject>();

        public DomainObject()
        {
        }

        public DomainObject(ISet<DomainPropertyValue> propertyValues) : base(propertyValues)
        {
        }

        public virtual ISet<DomainObject> Children
        {
            get { return _children; }
            protected set { _children = value; }
        }

        public virtual DomainObject Parent { get; protected set; }

        public virtual string Name
        {
            get
            {
                if (!HasProperty(DomainPropertyDefinition.NameId))
                    return "";
                return Convert.ToString(GetPropertyValue(DomainPropertyDefinition.NameId));
            }
            set { throw new NotImplementedException(); }
        }

        public virtual void AddChild(DomainObject item)
        {
            _children.Add(item);
            item.Parent = this;
        }
    }
}