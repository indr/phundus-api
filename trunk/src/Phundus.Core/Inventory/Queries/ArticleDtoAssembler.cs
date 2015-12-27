namespace Phundus.Core.Inventory.Queries
{
    using System;
    using System.Collections.Generic;
    using Articles.Model;
    using Infrastructure;

    /// <summary>
    /// Die <c>ArticleDtoAssembler</c>-Klasse wandelt Article-Domain-Objects in Article-DTOs.
    /// </summary>
    public class ArticleDtoAssembler
    {
        /// <summary>
        /// Assembliert die übergebenen Domain-Objects in neue DTOs.
        /// </summary>
        /// <param name="subjects">Die zu assemblierende Domain-Objects.</param>
        /// <returns></returns>
        public ArticleDto[] CreateDtos(IEnumerable<Article> subjects)
        {
            Guard.Against<ArgumentNullException>(subjects == null, "subjects");

            var result = new List<ArticleDto>();
            foreach (var each in subjects)
                result.Add(CreateDto(each));
            return result.ToArray();
        }

        /// <summary>
        /// Assembliert das übergebene Domain-Object in ein neues DTO.
        /// </summary>
        /// <param name="subject">Das zu assemblierende Domain-Object.</param>
        /// <returns></returns>
        public ArticleDto CreateDto(Article subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new ArticleDto();
            result.Id = subject.Id;
            result.Version = subject.Version;
            result.OrganizationId = subject.Owner.OwnerId.Value;
            result.OrganizationName = subject.Owner.Name;

            result.CreatedOn = subject.CreateDate;
            result.Name = subject.Caption;
            result.Brand = subject.Brand;
            result.Price = subject.Price;
            result.Description = subject.Description;
            result.Specification = subject.Specification;
            result.GrossStock = subject.GrossStock;
            result.Color = subject.Color;


            foreach (var each in subject.Images)
                result.AddImage(new ImageAssembler().CreateDto(each));
            return result;
        }
    }
}