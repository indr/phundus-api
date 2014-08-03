namespace Phundus.Core.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Cqrs;
    using Cqrs.Paging;
    using Inventory.Queries;

    public interface IShopArticleQueries
    {
        ShopArticleDetailDto GetArticle(int id);
        PagedResult<ShopArticleSearchResultDto> FindArticles(PageRequest pageRequest, string query, int? organization);
    }

    public class ShopArticleReadModel : ReadModelBase, IShopArticleQueries
    {
        static ShopArticleReadModel()
        {
            Mapper.CreateMap<IDataReader, ShopArticleDetailDto>();
            Mapper.CreateMap<IDataReader, ShopArticleSearchResultDto>();
            Mapper.CreateMap<IDataReader, ShopArticleImageDto>();
        }

        public ShopArticleDetailDto GetArticle(int id)
        {
            var result = Single<ShopArticleDetailDto>(
                @"select a.Id, a.Name, a.Price, a.Description, a.Specification,  o.Id as OrganizationId, o.Name as OrganizationName " +
                @"from [Article] a inner join [Organization] o on a.OrganizationId = o.Id " +
                @"where a.Id = {0}",
                id);

            // TODO: Select 1+1
            var images = Many<ShopArticleImageDto>(
                @"select FileName, [Type], [Length] from [Image] where ArticleId = {0}", id);

            result.Images =
                images.Where(p => p.Type.StartsWith("image", StringComparison.InvariantCultureIgnoreCase)).ToList();
            result.Documents =
                images.Where(p => !p.Type.StartsWith("image", StringComparison.InvariantCultureIgnoreCase))
                    .Select(each => new ShopArticleDocumentDto
                    {
                        FileName = each.FileName,
                        Type = each.Type,
                        Length = each.Length
                    }).ToList();


            return result;
        }

        public PagedResult<ShopArticleSearchResultDto> FindArticles(PageRequest pageRequest, string query,
            int? organization)
        {
            var result = Paged<ShopArticleSearchResultDto>(
                @"select a.Id, a.Name, a.Price, o.Name as OrganizationName " +
                @"from [Article] a " +
                @"inner join [Organization] o on a.OrganizationId = o.Id " +
                @"order by a.CreateDate desc",
                pageRequest);

            // TODO: Select N+1
            foreach (var each in result.Items)
            {
                var images = Many<ShopArticleImageDto>(
                    @"select FileName, [Type], [Length] from [Image] where ArticleId = {0}", each.Id);

                var image =
                    images.OrderBy(p => p.IsPreview)
                        .FirstOrDefault(p => p.Type.StartsWith("image", StringComparison.InvariantCultureIgnoreCase));
                
                if (image != null)
                    each.ImageFileName = image.FileName;
            }

            return result;
        }
    }

    public class ShopArticleDetailDto
    {
        private ICollection<ShopArticleDocumentDto> _documents = new Collection<ShopArticleDocumentDto>();
        private ICollection<ShopArticleImageDto> _images = new Collection<ShopArticleImageDto>();

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }

        public ICollection<ShopArticleImageDto> Images
        {
            get { return _images; }
            set { _images = value; }
        }

        public ICollection<ShopArticleDocumentDto> Documents
        {
            get { return _documents; }
            set { _documents = value; }
        }
    }

    public class ShopArticleSearchResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
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

    public class ShopArticleDocumentDto
    {
        public string FileName { get; set; }
        public string Type { get; set; }
        public long Length { get; set; }
    }
}