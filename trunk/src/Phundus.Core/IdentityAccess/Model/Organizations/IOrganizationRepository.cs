namespace Phundus.IdentityAccess.Model.Organizations
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Organizations.Model;

    public interface IOrganizationRepository : IRepository<Organization>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Organization GetById(Guid id);
        Organization GetById(OrganizationId organizationId);

        Organization FindById(Guid id);
        Organization FindById(OrganizationId organizationId);
        Organization FindByName(string name);
        ICollection<Organization> FindAll();
    }
}