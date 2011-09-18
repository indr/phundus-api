using Iesi.Collections.Generic;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class Model : BasePropertyEntity
    {
        private ISet<Model> _children = new HashedSet<Model>();

        public virtual ISet<Model> Children
        {
            get { return _children; }
            protected set { _children = value; }
        }

        public virtual Model Parent { get; protected set; }

        public virtual void AddChild(Model item)
        {
            _children.Add(item);
            item.Parent = this;
        }
    }
}