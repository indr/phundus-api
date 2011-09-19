using System;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Assembler
{
    public class ArticleAssembler
    {
        public static Article CreateDomainObject(object subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new Article();
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
            return WriteProperties(subject, result);
        }
    }
}