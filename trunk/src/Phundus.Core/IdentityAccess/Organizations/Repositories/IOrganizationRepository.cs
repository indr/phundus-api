namespace Phundus.IdentityAccess.Organizations.Repositories
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Infrastructure;
    using Model;

    public interface IOrganizationRepository : IRepository<Organization>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Organization GetById(Guid id);

        Organization FindById(Guid id);
        Organization FindByName(string name);
        ICollection<Organization> FindAll();
    }
}