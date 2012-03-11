using System;
using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Assembler
{
    public class ArticleDtoAssembler
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
            foreach (var each in subject.Images)
                result.AddImage(new ImageAssembler().CreateDto(each));
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
        /// Assembliert die Properties des übergebenen Domain-Objects in das übergebene DTO.
        /// </summary>
        /// <param name="subject">Das zu assemblierende Domain-Object.</param>
        /// <param name="result">Das zu aktualisierende DTO.</param>
        private static void WriteProperties(FieldedEntity subject, BasePropertiesDto result)
        {
            foreach (var each in subject.FieldValues)
            {
                var dtoProperty = new FieldValueDto();
                dtoProperty.PropertyId = each.FieldDefinition.Id;
                dtoProperty.Caption = each.FieldDefinition.Name;
                switch (each.FieldDefinition.DataType)
                {
                    case DataType.Boolean:
                        dtoProperty.DataType = FieldDataType.Boolean;
                        break;
                    case DataType.Text:
                        dtoProperty.DataType = FieldDataType.Text;
                        break;
                    case DataType.Integer:
                        dtoProperty.DataType = FieldDataType.Integer;
                        break;
                    case DataType.Decimal:
                        dtoProperty.DataType = FieldDataType.Decimal;
                        break;
                    case DataType.DateTime:
                        dtoProperty.DataType = FieldDataType.DateTime;
                        break;
                    case DataType.RichText:
                        dtoProperty.DataType = FieldDataType.RichText;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("FieldDefinition.DataType",
                                                              "Datentypen müssen in den Klasse DataType und FieldDataType übereinstimmen");
                }
                dtoProperty.ValueId = each.Id;
                dtoProperty.Value = each.Value;
                dtoProperty.IsDiscriminator = each.IsDiscriminator;

                result.AddProperty(dtoProperty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="result"></param>
        private static void CreateChildren(CompositeEntity subject, ArticleDto result)
        {
            foreach (var each in subject.Children)
            {
                // TODO: Generics anstelle Cast?
                result.AddChild(CreateDto((Article) each));
            }
        }
    }
}