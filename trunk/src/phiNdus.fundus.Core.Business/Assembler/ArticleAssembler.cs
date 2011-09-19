using System;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Assembler
{
    public class ArticleAssembler
    {
        public static Article CreateDomainObject(ArticleDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new Article();
            WriteProperties(subject, result);
            return result;
        }

        private static Article WriteProperties(ArticleDto subject, Article result)
        {
            var propertyDefinitionRepo = IoC.Resolve<IDomainPropertyDefinitionRepository>();
            foreach (var each in subject.Properties)
            {
                result.AddProperty(propertyDefinitionRepo.Get(each.PropertyId), each.Value);
            }
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
    }
}