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
        

        /// <summary>
        /// Assembliert das übergebene Domain-Object in ein neues DTO.
        /// </summary>
        /// <param name="subject">Das zu assemblierende Domain-Object.</param>
        /// <returns></returns>
        public static ArticleDto CreateDto(Article subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new ArticleDto();
            result.Id = subject.Id;
            result.Version = subject.Version;
            WriteProperties(subject, result);
            CreateChildren(subject, result);
            return result;
        }

        /// <summary>
        /// Assembliert die übergebenen Domain-Objects in neue DTOs.
        /// </summary>
        /// <param name="subjects">Die zu assemblierende Domain-Objects.</param>
        /// <returns></returns>
        public static ArticleDto[] CreateDtos(ICollection<Article> subjects)
        {
            Guard.Against<ArgumentNullException>(subjects == null, "subjects");

            var result = new List<ArticleDto>();
            foreach (var each in subjects)
                result.Add(CreateDto(each));
            return result.ToArray();
        }

        /// <summary>
        /// Wandelt das übergebene Article-DTO-Objekt in ein neues
        /// Article-Domain-Objekt um.
        /// </summary>
        /// <param name="subject">Das zu assemblierende DTO-Objekt</param>
        /// <returns></returns>
        public static Article CreateDomainObject(ArticleDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new Article();
            WriteProperties(subject, result);
            CreateChildren(subject, result);
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

            var result = IoC.Resolve<IArticleRepository>().Get(subject.Id);
            Guard.Against<EntityNotFoundException>(result == null, "Article entity not found");
            Guard.Against<DtoOutOfDateException>(result.Version != subject.Version, "Dto is out of date");

            WriteProperties(subject, result);
            UpdateChildren(subject, result);
            return result;
        }

        /// <summary>
        /// Assembliert die Properties des übergebenen DTOs in das übergebene Domain-Object.
        /// </summary>
        /// <param name="subject">Das zu assemblierende DTO.</param>
        /// <param name="result">Das zu aktualisierende Domain-Object.</param>
        private static void WriteProperties(BasePropertiesDto subject, BasePropertyEntity result)
        {
            // Neue Properties hinzufügen, oder bestehende Property-Values aktualisieren.
            var propertyDefinitionRepo = IoC.Resolve<IDomainPropertyDefinitionRepository>();
            foreach (var each in subject.Properties)
            {
                if (result.HasProperty(each.PropertyId))
                    result.SetPropertyValue(each.PropertyId, each.Value);
                else
                    result.AddProperty(propertyDefinitionRepo.Get(each.PropertyId), each.Value);
            }

            // Properties, die nicht mehr im DTO vorhanden sind, entfernen.
            var propertiesToRemove = new List<DomainPropertyValue>();
            foreach (var each in result.PropertyValues)
            {
                if (subject.Properties.FirstOrDefault(x => x.PropertyId == each.PropertyDefinition.Id) == null)
                    propertiesToRemove.Add(each);
            }
            foreach (var each in propertiesToRemove)
                result.RemoveProperty(each.PropertyDefinition);
        }

        /// <summary>
        /// Assembliert die Properties des übergebenen Domain-Objects in das übergebene DTO.
        /// </summary>
        /// <param name="subject">Das zu assemblierende Domain-Object.</param>
        /// <param name="result">Das zu aktualisierende DTO.</param>
        private static void WriteProperties(BasePropertyEntity subject, BasePropertiesDto result)
        {
            foreach (var each in subject.PropertyValues)
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="result"></param>
        private static void CreateChildren(DomainObject subject, ArticleDto result)
        {
            foreach (var each in subject.Children)
            {
                // TODO: Generics anstelle Cast?
                result.AddChild(CreateDto((Article) each));
            }
        }

        private static void CreateChildren(ArticleDto subject, Article result)
        {
            foreach (var each in subject.Children)
            {
                var child = new Article();
                WriteProperties(each, child);
                result.AddChild(child);
            }
        }

        private static void UpdateChildren(ArticleDto subject, Article result)
        {
            // Neue Childs hinzufügen, oder bestehende Updaten
            foreach (var each in subject.Children)
            {
                Article child = null;
                if (each.Id > 0)
                    child = UpdateDomainObject(each);
                else
                    child = CreateDomainObject(each);
                result.AddChild(child);
            }

            // Children, die nicht mehr im DTO vorhanden sind, entfernen.
            var childrenToRemove = new List<DomainObject>();
            foreach (var each in result.Children)
            {
                if (subject.Children.SingleOrDefault(x => x.Id == each.Id) == null)
                    childrenToRemove.Add(each);
            }
            foreach (var each in childrenToRemove)
                result.RemoveChild((Article)each);
        }
    }
}