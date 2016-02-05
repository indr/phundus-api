﻿namespace Phundus.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Cqrs;
    using Cqrs.Paging;

    public interface IShopArticleQueries
    {
        ShopArticleDetailDto GetArticle(int id);
        PagedResult<ShopArticleSearchResultDto> FindArticles(PageRequest pageRequest, string query, Guid? organization);
    }

    public class ShopArticleReadModel : AutoMappingReadModelBase, IShopArticleQueries
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
                @"select a.Id, a.ArticleGuid, a.Name, a.PublicPrice, a.MemberPrice, a.Description, a.Specification,  a.Owner_OwnerId as OrganizationId, a.Owner_Name as OrganizationName " +
                @"from [Dm_Inventory_Article] a left join [Dm_IdentityAccess_Organization] o on (a.Owner_OwnerId = o.Guid) " +
                @"where a.Id = {0} " +
                //@" and o.[Plan] > 0" +
                @"",
                id);

            // TODO: Select 1+1
            var images = Many<ShopArticleImageDto>(
                @"select FileName, [Type], [Length] from [Dm_Inventory_ArticleFile] where ArticleId = {0}", id);

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
            Guid? organization)
        {
            var where = "where 1 = 1 ";

            // TODO: #48 SQL-Injection
            if (!String.IsNullOrWhiteSpace(query))
                where = where + string.Format(" and a.Name like '%{0}%' ", query.Replace("'", "''"));

            if (organization.HasValue)
                where = where + string.Format(" and a.Owner_OwnerId = '{0}' ", organization.Value);


            var result = Paged<ShopArticleSearchResultDto>(
                @"select a.Id, a.Name, a.PublicPrice, a.MemberPrice, a.Owner_Name as OrganizationName " +
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

    public class ShopArticleDetailDto
    {
        private ICollection<ShopArticleDocumentDto> _documents = new Collection<ShopArticleDocumentDto>();
        private ICollection<ShopArticleImageDto> _images = new Collection<ShopArticleImageDto>();

        public int Id { get; set; }
        public Guid ArticleGuid { get; set; }
        public string Name { get; set; }
        public decimal PublicPrice { get; set; }
        public decimal? MemberPrice { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public Guid OrganizationId { get; set; }
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

    public class ShopArticleDocumentDto
    {
        public string FileName { get; set; }
        public string Type { get; set; }
        public long Length { get; set; }
    }
}