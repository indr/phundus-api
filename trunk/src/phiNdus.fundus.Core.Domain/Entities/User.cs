﻿using System;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class User : BaseEntity
    {
        private string _firstName;
        private string _lastName;
        private Membership _membership;
        private Role _role;

        public User() : this(0)
        {
        }

        public User(int id) : this(id, 0)
        {
        }

        public User(int id, int version) : base(id, version)
        {
            _firstName = "";
            _lastName = "";
            _membership = new Membership();
            _membership.User = this;
            _role = Role.User;
        }

        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public virtual Membership Membership
        {
            get { return _membership; }
            set { _membership = value; }
        }

        public virtual Role Role
        {
            get { return _role; }
            set { _role = value; }
        }

        public string DisplayName { get { return String.Format("{0} {1}", FirstName, LastName); } }
    }
}