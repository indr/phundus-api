namespace phiNdus.fundus.Core.Domain
{
    public class User : BaseEntity
    {
        private string _firstName = "";
        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private string _lastName = "";
        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
    }
}