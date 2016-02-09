namespace Phundus.Shop.Queries
{
    using System;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Cqrs;
    using Cqrs.Paging;

    public interface IShopArticleQueries
    {
        PagedResult<ShopArticleSearchResultDto> FindArticles(PageRequest pageRequest, string query, Guid? organization);
    }

    public class ShopArticleReadModel : AutoMappingReadModelBase, IShopArticleQueries
    {
        static ShopArticleReadModel()
        {
            Mapper.CreateMap<IDataReader, ShopArticleImageDto>();
            Mapper.CreateMap<IDataReader, ShopArticleSearchResultDto>();
        }

        public PagedResult<ShopArticleSearchResultDto> FindArticles(PageRequest pageRequest, string query,
            Guid? organization)
        {
            var where = "where 1 = 1 ";

            // TODO: #48 SQL-Injection
            if (!String.IsNullOrWhiteSpace(query))
                where = where + string.Format(" and a.Name like '%{0}%' ", query.Replace("'", "''"));

            if (organization.HasValue)
                where = where + string.Format(" and a.Owner_OwnerId = '{0}' ", organization.Value);


            var result = Paged<ShopArticleSearchResultDto>(
                @"select a.Id, a.ArticleGuid, a.Name, a.PublicPrice, a.MemberPrice, a.Owner_Name as OrganizationName " +
                @"from [Dm_Inventory_Article] a " +
                @"left join [Dm_IdentityAccess_Organization] o on (a.Owner_OwnerId = o.Guid) " +
                where +
                //@" and o.[Plan] > 0 " +
                @"order by a.CreateDate desc, a.Id desc ",
                pageRequest);

            // TODO: Select N+1
            foreach (var each in result.Items)
            {
                var images = Many<ShopArticleImageDto>(
                    @"select FileName, [Type], [Length], [IsPreview] from [Dm_Inventory_ArticleFile] where ArticleId = {0}",
                    each.Id)
                    .Where(p => p.Type.StartsWith("image", StringComparison.InvariantCultureIgnoreCase)).ToList();

                var image = images.FirstOrDefault(p => p.IsPreview);

                if (image == null)
                    image = images.FirstOrDefault();

                if (image != null)
                    each.ImageFileName = image.FileName;
            }

            return result;
        }
    }

    public class ShopArticleSearchResultDto
    {
        public int Id { get; set; }
        public Guid ArticleGuid { get; set; }
        public string Name { get; set; }
        public decimal PublicPrice { get; set; }
        public decimal? MemberPrice { get; set; }
        public string OrganizationName { get; set; }
        public string ImageFileName { get; set; }
    }

    public class ShopArticleImageDto
    {
        public string FileName { get; set; }
        public string Type { get; set; }
        public long Length { get; set; }
        public bool IsPreview { get; set; }
    }
}