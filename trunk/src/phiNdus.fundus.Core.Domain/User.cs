namespace phiNdus.fundus.Core.Domain
{
    public class User : BaseEntity
    {
        public User() : this(0)
        {
        }

        public User(int id) : base(id)
        {
            FirstName = "";
            LastName = "";
            Membership = new Membership();
            Membership.User = this;
        }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual Membership Membership { get; set; }
    }
}