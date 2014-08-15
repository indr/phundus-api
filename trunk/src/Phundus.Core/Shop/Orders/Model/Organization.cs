namespace Phundus.Core.Shop.Orders.Model
{
    using Ddd;

    public class Organization : ValueObject
    {
        private int _id;
        private string _name;

        protected Organization()
        {
            
        }

        public Organization(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public virtual int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }
    }
}