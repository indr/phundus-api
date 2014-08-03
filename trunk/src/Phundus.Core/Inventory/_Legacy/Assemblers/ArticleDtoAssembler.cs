namespace Phundus.Core.Inventory._Legacy.Assemblers
{
    using System;
    using System.Collections.Generic;
    using Dtos;
    using IdentityAndAccess.Organizations.Repositories;
    using Infrastructure;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Queries;

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
        public ArticleDto[] CreateDtos(ICollection<Article> subjects)
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

            var organizationRepository = ServiceLocator.Current.GetInstance<IOrganizationRepository>();

            var result = new ArticleDto();
            result.Id = subject.Id;
            result.Version = subject.Version;
            var organization = organizationRepository.ById(subject.OrganizationId);
            result.OrganizationId = organization.Id;
            result.OrganizationName = organization.Name;

            result.CreatedOn = subject.CreateDate;
            result.Name = subject.Name;
            result.Brand = subject.Brand;
            result.Price = subject.Price;
            result.OrganizationId = subject.OrganizationId;
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