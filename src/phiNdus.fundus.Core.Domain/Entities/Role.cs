namespace phiNdus.fundus.Core.Domain.Entities
{
    public class Role : BaseEntity
    {
        public static readonly Role User = new Role(1, "Benutzer");
        public static readonly Role Administrator = new Role(2, "Administrator");
        private string _name;

        public Role()
        {
        }

        private Role(int id, string name) : base(id)
        {
            _name = name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            var that = obj as Role;
            if (that == null) return false;
            if (this.Name != that.Name)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}