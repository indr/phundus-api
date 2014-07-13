namespace phiNdus.fundus.Web.Business.Assembler
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Domain.Repositories;
    using phiNdus.fundus.Web.Business.Dto;
    using piNuts.phundus.Infrastructure;

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

            var result = new ArticleDto();
            result.Id = subject.Id;
            result.Version = subject.Version;
            result.OrganizationName = subject.Organization.Name;
            WriteFields(subject, result);
            CreateChildren(subject, result);
            foreach (var each in subject.Images)
                result.AddImage(new ImageAssembler().CreateDto(each));
            return result;
        }

        /// <summary>
        /// Assembliert die Properties des übergebenen Domain-Objects in das übergebene DTO.
        /// </summary>
        /// <param name="subject">Das zu assemblierende Domain-Object.</param>
        /// <param name="result">Das zu aktualisierende DTO.</param>
        private void WriteFields(Article subject, BasePropertiesDto result)
        {
            foreach (var each in subject.FieldValues)
                WriteField(each, result);

            WriteField(FieldDefinition.CreateDateId, subject.CreateDate, result);
            //WriteField(FieldDefinition.NetStockId, subject.ReservableStock, result);
        }

        private void WriteField(int fieldDefinitionId, object value, BasePropertiesDto result)
        {
            var fieldDefinition = ServiceLocator.Current.GetInstance<IFieldDefinitionRepository>().ById(fieldDefinitionId);
            var fieldValueDto = CreateFieldValueDto(fieldDefinition);
            fieldValueDto.Value = value;
            result.AddProperty(fieldValueDto);
        }

        private void WriteField(FieldValue subject, BasePropertiesDto result)
        {
            var fieldValueDto = CreateFieldValueDto(subject.FieldDefinition);
            fieldValueDto.ValueId = subject.Id;
            fieldValueDto.Value = subject.Value;
            fieldValueDto.IsDiscriminator = subject.IsDiscriminator;

            result.AddProperty(fieldValueDto);
        }

        private FieldValueDto CreateFieldValueDto(FieldDefinition fieldDefinition)
        {
            var result = new FieldValueDto();
            result.PropertyId = fieldDefinition.Id;
            result.Caption = fieldDefinition.Name;
            result.IsCalculated = !fieldDefinition.IsAttachable;
            result.Position = fieldDefinition.Position;
            switch (fieldDefinition.DataType)
            {
                case DataType.Boolean:
                    result.DataType = FieldDataType.Boolean;
                    break;
                case DataType.Text:
                    result.DataType = FieldDataType.Text;
                    break;
                case DataType.Integer:
                    result.DataType = FieldDataType.Integer;
                    break;
                case DataType.Decimal:
                    result.DataType = FieldDataType.Decimal;
                    break;
                case DataType.DateTime:
                    result.DataType = FieldDataType.DateTime;
                    break;
                case DataType.RichText:
                    result.DataType = FieldDataType.RichText;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("FieldDefinition.DataType",
                                                          "Datentypen müssen in den Klasse DataType und FieldDataType übereinstimmen");
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="result"></param>
        private void CreateChildren(CompositeEntity subject, ArticleDto result)
        {
            foreach (var each in subject.Children)
            {
                // TODO: Generics anstelle Cast?
                result.AddChild(CreateDto((Article) each));
            }
        }
    }
}