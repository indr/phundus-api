using System;
using Iesi.Collections.Generic;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

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

        public DomainObject(int id, int version) : base(id, version)
        {
        }

        public virtual ISet<DomainObject> Children
        {
            get { return _children; }
            protected set { _children = value; }
        }

        public virtual DomainObject Parent { get; protected set; }

        public virtual string Caption
        {
            get
            {
                return !HasProperty(DomainPropertyDefinition.CaptionId)
                           ? ""
                           : Convert.ToString(GetPropertyValue(DomainPropertyDefinition.CaptionId));
            }
            set
            {
                if (!HasProperty(DomainPropertyDefinition.CaptionId))
                    AddProperty(
                        IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(DomainPropertyDefinition.CaptionId));
                SetPropertyValue(DomainPropertyDefinition.CaptionId, value);
            }
        }

        public virtual void AddChild(DomainObject child)
        {
            _children.Add(child);
            child.Parent = this;
        }

        public virtual void RemoveChild(DomainObject child)
        {
            Children.Remove(child);
            child.Parent = null;
        }

        public virtual bool HasChildren
        {
            get { return Children.Count > 0; }
        }
    }
}