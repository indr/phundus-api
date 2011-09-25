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
            result = WriteDomainObject(subject, result);
            result = CreateChildObjects(subject, result);
            return result;
        }

        private static Article CreateChildObjects(ArticleDto subject, Article result)
        {
            foreach (var each in subject.Children)
            {
                var child = new Article();
                child = WriteDomainObject(each, child);
                result.AddChild(child);
            }
            return result;
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
                switch (each.PropertyDefinition.DataType)
                {
                    case DomainPropertyType.Boolean:
                        dtoProperty.DataType = PropertyDataType.Boolean;
                        break;
                    case DomainPropertyType.Text:
                        dtoProperty.DataType = PropertyDataType.Text;
                        break;
                    case DomainPropertyType.Integer:
                        dtoProperty.DataType = PropertyDataType.Integer;
                        break;
                    case DomainPropertyType.Decimal:
                        dtoProperty.DataType = PropertyDataType.Decimal;
                        break;
                    case DomainPropertyType.DateTime:
                        dtoProperty.DataType = PropertyDataType.DateTime;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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
            result = WriteChildren(subject, result);
            return result;
        }

        private static ArticleDto WriteChildren(Article subject, ArticleDto result)
        {
            foreach (var each in subject.Children) {
                // TODO: Generics anstelle Cast?
                result.AddChild(CreateDto((Article)each));
            }
            return result;
        }

        public static Article UpdateDomainObject(ArticleDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = IoC.Resolve<IArticleRepository>().Get(subject.Id);
            Guard.Against<EntityNotFoundException>(result == null, "Article entity not found");
            Guard.Against<DtoOutOfDateException>(result.Version != subject.Version, "Dto is out of date");

            result = WriteDomainObject(subject, result);
            result = WriteChildren(subject, result);
            return result;
        }

        private static Article WriteChildren(ArticleDto subject, Article result)
        {
            foreach (var each in subject.Children)
            {
                // Hier kompliziert =)
            }
            return subject;
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