using System;
using System.Collections.Generic;
using System.Linq;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Assembler
{
    public class ArticleAssembler
    {
        private static Article WriteDomainObject(ArticleDto subject, Article result)
        {
            WriteProperties(subject, result);
            return result;
        }

        public static Article CreateDomainObject(ArticleDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new Article();
            return WriteDomainObject(subject, result);
        }

        private static Article WriteProperties(ArticleDto subject, Article result)
        {
            var propertyDefinitionRepo = IoC.Resolve<IDomainPropertyDefinitionRepository>();
            foreach (var each in subject.Properties)
            {
                if (result.HasProperty(each.PropertyId))
                    result.SetPropertyValue(each.PropertyId, each.Value);
                else
                    result.AddProperty(propertyDefinitionRepo.Get(each.PropertyId), each.Value);
            }

            var propertiesToRemove = new List<DomainPropertyValue>();
            foreach (var each in result.PropertyValues)
            {
                if (subject.Properties.FirstOrDefault(x => x.PropertyId == each.PropertyDefinition.Id) == null)
                    propertiesToRemove.Add(each);
            }
            foreach (var each in propertiesToRemove)
                result.RemoveProperty(each.PropertyDefinition);
            return result;
        }

        private static ArticleDto WriteProperties(Article subject, ArticleDto result)
        {
            foreach(var each in subject.PropertyValues)
            {
                var dtoProperty = new DtoProperty();
                dtoProperty.PropertyId = each.PropertyDefinition.Id;
                dtoProperty.Caption = each.PropertyDefinition.Name;

                dtoProperty.ValueId = each.Id;
                dtoProperty.Value = each.Value;

                result.AddProperty(dtoProperty);
            }


            return result;
        }

        public static ArticleDto CreateDto(Article subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new ArticleDto();
            result.Id = subject.Id;
            result.Version = subject.Version;
            result = WriteProperties(subject, result);
            return result;
        }

        public static Article UpdateDomainObject(ArticleDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = IoC.Resolve<IArticleRepository>().Get(subject.Id);
            Guard.Against<EntityNotFoundException>(result == null, "Article entity not found");
            Guard.Against<DtoOutOfDateException>(result.Version != subject.Version, "Dto is out of date");

            return WriteDomainObject(subject, result);
        }

        public static ArticleDto[] CreateDtos(ICollection<Article> subjects)
        {
            Guard.Against<ArgumentNullException>(subjects == null, "subjects");

            var result = new List<ArticleDto>();
            foreach (var each in subjects)
                result.Add(CreateDto(each));
            return result.ToArray();
        }
    }
}