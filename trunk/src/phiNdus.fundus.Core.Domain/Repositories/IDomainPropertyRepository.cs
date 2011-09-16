using System.Collections.Generic;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public interface IDomainPropertyRepository : IRepository<DomainProperty>
    {
        ICollection<DomainProperty> FindAll();
    }
}