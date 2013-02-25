﻿using System;
using Iesi.Collections.Generic;

namespace phiNdus.fundus.Domain.Entities
{
    public class User : EntityBase
    {
        private string _firstName;
        private string _lastName;
        private Membership _membership;
        private Role _role;
        private int? _jsNumber;

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

        public virtual string DisplayName { get { return String.Format("{0} {1}", FirstName, LastName); } }

        public virtual int? JsNumber
        {
            get {
                return _jsNumber;
            }
            set {

                if (value.HasValue && ((value > 9999999) || (value < 1)))
                    throw new ArgumentOutOfRangeException("value", "Die J+S-Nummer muss sechs- oder siebenstellig sein.");
                _jsNumber = value;
            }
        }

        public virtual string MobileNumber { get; set; }

        public virtual void Join(Organization organization)
        {
            var membership = new OrganizationMembership();
            membership.Organization = organization;
            membership.User = this;
            Memberships.Add(membership);
        }

        private ISet<OrganizationMembership> _memberships = new HashedSet<OrganizationMembership>();

        public virtual ISet<OrganizationMembership> Memberships
        {
            get { return _memberships; }
            set { _memberships = value; }
        }

        public virtual string Street { get; set; }
        public virtual string Postcode { get; set; }
        public virtual string City { get; set; }
    }
}