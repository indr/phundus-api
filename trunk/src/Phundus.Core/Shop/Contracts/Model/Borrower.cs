namespace Phundus.Core.Shop.Contracts.Model
{
    using Ddd;

    public class Borrower : ValueObject
    {
        private string _email;
        private string _firstName;
        private int _id;
        private string _lastName;

        protected Borrower()
        {
        }

        public Borrower(int id, string firstName, string lastName, string email)
        {
            _id = id;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
        }

        public virtual int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string FirstName
        {
            get { return _firstName; }
            protected set { _firstName = value; }
        }

        public virtual string LastName
        {
            get { return _lastName; }
            protected set { _lastName = value; }
        }

        public virtual string Email
        {
            get { return _email; }
            protected set { _email = value; }
        }
    }
}