namespace Phundus.Core.InventoryCtx
{
    using Iesi.Collections.Generic;

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

        public virtual bool HasChildren
        {
            get { return Children.Count > 0; }
        }

        public virtual bool AddChild(CompositeEntity child)
        {
            var result = _children.Add(child);
            if (result)
                child.Parent = this;
            return result;
        }

        public virtual bool RemoveChild(CompositeEntity child)
        {
            var result = Children.Remove(child);
            if (result)
                child.Parent = null;
            return result;
        }

        
    }
}