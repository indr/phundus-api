namespace Phundus.Core.Inventory._Legacy.Assemblers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dtos;
    using Infrastructure;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Queries;
    using Repositories;

    /// <summary>
    /// Die <c>ArticleDomainAssembler</c> wandelt Article-DTOs in Article-Domain-Objects.
    /// </summary>
    public class ArticleDomainAssembler
    {
        /// <summary>
        /// Wandelt das übergebene Article-DTO-Objekt in ein neues
        /// Article-Domain-Objekt um.
        /// </summary>
        /// <param name="subject">Das zu assemblierende DTO-Objekt</param>
        /// <returns></returns>
        public static Article CreateDomainObject(ArticleDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new Article(subject.OrganizationId, subject.Name);

            
            result.Brand = subject.Brand;
            result.Price = subject.Price;
            
            result.Description = subject.Description;
            result.Specification = subject.Specification;
            result.GrossStock = subject.GrossStock;
            result.Color = subject.Color;

            return result;
        }

        /// <summary>
        /// Assembliert das übergebene DTO in das korrespondierende Domain-Object,
        /// welches zuerst aus dem Repository geladen wird.
        /// </summary>
        /// <param name="subject">Das zu assemblierende DTO.</param>
        /// <returns></returns>
        public static Article UpdateDomainObject(ArticleDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = ServiceLocator.Current.GetInstance<IArticleRepository>().ById(subject.Id);
            Guard.Against<EntityNotFoundException>(result == null, "Article entity not found");
            Guard.Against<DtoOutOfDateException>(result.Version != subject.Version, "Dto is out of date");

            result.Caption = subject.Name;
            result.Brand = subject.Brand;
            result.Price = subject.Price;
            result.Description = subject.Description;
            result.Specification = subject.Specification;
            result.GrossStock = subject.GrossStock;
            result.Color = subject.Color;

            return result;
        }
    }
}