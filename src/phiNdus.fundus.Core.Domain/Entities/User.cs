namespace phiNdus.fundus.Core.Domain.Entities
{
    public class User : BaseEntity
    {
        public User() : this(0)
        {
            
        }

        public User(int id) : base(id)
        {
            _firstName = "";
            _lastName = "";
            _membership = new Membership();
            _membership.User = this;
        }

        private string _firstName;
        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private string _lastName;
        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        private Membership _membership;
        public virtual Membership Membership
        {
            get { return _membership; }
            set { _membership = value; }
        }
    }
}