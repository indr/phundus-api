﻿namespace Phundus.Core.Shop.Queries
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using AutoMapper;
    using Cqrs;
    using Cqrs.Paging;
    using Inventory.Queries;
    using Remotion.Linq.Parsing.Structure.IntermediateModel;

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
        }

        public IArticleQueries ArticleQueries { get; set; }

        public ShopArticleDetailDto GetArticle(int id)
        {
            return Single<ShopArticleDetailDto>(
                @"select a.Id, a.Name, a.Price, a.Description, a.Specification,  o.Id as OrganizationId, o.Name as OrganizationName " +
                @"from [Article] a inner join [Organization] o on a.OrganizationId = o.Id " +
                @"where a.Id = {0}",
                id);
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
                var fileName = ExecuteScalar<string>(string.Format(
                    "select top 1 FileName from [Image] where ArticleId = {0} and [Type] = 'image/jpeg' order by IsPreview desc", each.Id));
                each.ImageFileName = fileName;
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
    }

    public class ShopArticleDocumentDto
    {
        public string FileName { get; set; }
        public string DisplayLength { get; set; }
    }
}