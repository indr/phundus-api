namespace Phundus.Core.IdentityAndAccess.Users.Model
{
    using Ddd;

    public class Role : EntityBase
    {
        private static readonly Role _user = new Role(1, "User");
        public static Role User { get { return _user; } }

        private static readonly Role _chief = new Role(2, "Chief");
        public static Role Chief { get { return _chief; } }

        private static readonly Role _administrator = new Role(3, "Admin");
        public static Role Administrator { get { return _administrator; } }

        public Role()
        {
        }

        public Role(int id, string name) : base(id)
        {
            _name = name;
        }

        private string _name;
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            var that = obj as Role;
            if (that == null) return false;
            if (Name != that.Name)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}