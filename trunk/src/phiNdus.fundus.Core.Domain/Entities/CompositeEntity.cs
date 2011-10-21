using System;
using Iesi.Collections.Generic;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Entities
{
    /// <summary>
    /// Die Klasse CompositeEntity stellt Funktionen für Parent-Child-Beziehung zur Verfügung.
    /// </summary>
    public class CompositeEntity : FieldedEntity
    {
        private ISet<CompositeEntity> _children = new HashedSet<CompositeEntity>();

        public CompositeEntity()
        {
        }

        public CompositeEntity(ISet<FieldValue> fieldValues) : base(fieldValues)
        {
        }

        public CompositeEntity(int id, int version) : base(id, version)
        {
        }

        public virtual ISet<CompositeEntity> Children
        {
            get { return _children; }
            protected set { _children = value; }
        }

        public virtual CompositeEntity Parent { get; protected set; }



        public virtual void AddChild(CompositeEntity child)
        {
            _children.Add(child);
            child.Parent = this;
        }

        public virtual void RemoveChild(CompositeEntity child)
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