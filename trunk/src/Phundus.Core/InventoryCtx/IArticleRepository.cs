namespace Phundus.Core.InventoryCtx
{
    using System.Collections.Generic;
    using Infrastructure;
    using OrganizationAndMembershipCtx.Model;

    public interface IArticleRepository : IRepository<Article>
    {
        ICollection<Article> FindAll(Organization selectedOrganization);
        ICollection<Article> FindMany(string query, int? organization, int start, int count, out int total);
    }
}