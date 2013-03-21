using System;
using Iesi.Collections.Generic;

namespace phiNdus.fundus.Domain.Entities
{
    public class Organization : EntityBase
    {
        public virtual string Name { get; set; }

        private ISet<OrganizationMembership> _memberships = new HashedSet<OrganizationMembership>();

        public Organization()
        {
            
        }

        public Organization(int id) : base(id)
        {
            
        }

        public virtual ISet<OrganizationMembership> Memberships
        {
            get { return _memberships; }
            set { _memberships = value; }
        }
    }

    public class OrganizationMembership : EntityBase
    {
        public virtual User User { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual int Role { get; set; }

        
    }
}