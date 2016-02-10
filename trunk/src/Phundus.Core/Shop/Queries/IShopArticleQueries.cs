namespace Phundus.Shop.Queries
{
    using System;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Cqrs;
    using Cqrs.Paging;
    using Inventory.Queries;

    public interface IShopArticleQueries
    {
        PagedResult<ShopArticleSearchResultDto> FindArticles(PageRequest pageRequest, string query, Guid? organization);
    }

    public class ShopArticleReadModel : AutoMappingReadModelBase, IShopArticleQueries
    {
        private readonly IItemQueries _itemQueries;

        public ShopArticleReadModel(IItemQueries itemQueries)
        {
            if (itemQueries == null) throw new ArgumentNullException("itemQueries");
            _itemQueries = itemQueries;
        }

        static ShopArticleReadModel()
        {
            Mapper.CreateMap<IDataReader, ShopArticleImageDto>();
            Mapper.CreateMap<IDataReader, ShopArticleSearchResultDto>();
        }

        public PagedResult<ShopArticleSearchResultDto> FindArticles(PageRequest pageRequest, string query,
            Guid? organization)
        {
            var result = _itemQueries.Query(query, organization, pageRequest.Offset, pageRequest.Size);

            var pageResponse = new PageResponse
            {
                Size = result.Limit,
                Total = result.Total,
                Index = (int)Math.Floor((result.Offset + 1)/(double)result.Limit)
            };
            return new PagedResult<ShopArticleSearchResultDto>(pageResponse, result.Result.Select(s =>
                new ShopArticleSearchResultDto
                {
                    ArticleGuid = s.ArticleGuid,
                    Id = s.ArticleId,
                    ImageFileName = String.IsNullOrWhiteSpace(s.PreviewImageFileName) ? null : String.Format(@"~\Content\Images\Articles\{0}\{1}", s.ArticleId, s.PreviewImageFileName),
                    MemberPrice = s.MemberPrice,
                    Name = s.Name,
                    OrganizationName = s.OwnerName,
                    PublicPrice = s.PublicPrice
                }));
        }

        public PagedResult<ShopArticleSearchResultDto> FindArticles_Sql(PageRequest pageRequest, string query,
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