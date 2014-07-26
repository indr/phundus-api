﻿namespace Phundus.Core.IdentityAndAccess.Users.Model
{
    using System;
    using Ddd;
    using IdentityAndAccess.Organizations.Model;

    public class User : EntityBase
    {
        private Account _account;
        private string _firstName;
        private int? _jsNumber;
        private string _lastName;
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
            _account = new Account();
            _account.User = this;
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

        public virtual Account Account
        {
            get { return _account; }
            set { _account = value; }
        }

        public virtual Role Role
        {
            get { return _role; }
            set { _role = value; }
        }

        public virtual string DisplayName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        public virtual int? JsNumber
        {
            get { return _jsNumber; }
            set
            {
                if (value.HasValue && ((value > 9999999) || (value < 1)))
                    throw new ArgumentOutOfRangeException("value", "Die J+S-Nummer muss sechs- oder siebenstellig sein.");
                _jsNumber = value;
            }
        }

        public virtual string MobileNumber { get; set; }

        //public virtual void Join(Organization organization)
        //{
        //    var membership = new OrganizationMembership();
        //    membership.Organization = organization;
        //    membership.User = this;
        //    membership.Role = Role.User.Id;
        //    Memberships.Add(membership);
        //    SelectedOrganization = organization;
        //}

        //private ISet<OrganizationMembership> _memberships = new HashedSet<OrganizationMembership>();

        //public virtual ISet<OrganizationMembership> Memberships
        //{
        //    get { return _memberships; }
        //    set { _memberships = value; }
        //}

        public virtual string Street { get; set; }
        public virtual string Postcode { get; set; }
        public virtual string City { get; set; }

        //public virtual void SelectOrganization(Organization organization)
        //{
        //    foreach (var each in Memberships)
        //    {
        //        if (each.Organization == organization)
        //        {
        //            SelectedOrganization = each.Organization;
        //            return;
        //        }
        //    }
        //}

        public virtual Organization SelectedOrganization { get; set; }

        //public virtual bool IsChiefOf(Organization organization)
        //{
        //    if (organization == null)
        //        return false;

        //    return (from each in Memberships
        //            where each.Organization.Id == organization.Id
        //            select each.Role == Role.Chief.Id).FirstOrDefault();
        //}

        //public virtual bool IsMemberOf(Organization organization)
        //{
        //    if (organization == null)
        //        return false;

        //    return (from each in Memberships 
        //            where each.Organization.Id == organization.Id
        //            select each.Role >= Role.User.Id).FirstOrDefault();
        //}
    }
}