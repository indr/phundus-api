using System;
using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Assembler
{
    public class PropertyDefinitionAssembler
    {
        private static PropertyDataType ConvertDataType(DataType value)
        {
            switch (value)
            {
                case DataType.Boolean:
                    return PropertyDataType.Boolean;
                case DataType.Text:
                    return PropertyDataType.Text;
                case DataType.Integer:
                    return PropertyDataType.Integer;
                case DataType.Decimal:
                    return PropertyDataType.Decimal;
                case DataType.DateTime:
                    return PropertyDataType.DateTime;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        private static DataType ConvertDataType(PropertyDataType value)
        {
            switch (value)
            {
                case PropertyDataType.Boolean:
                    return DataType.Boolean;
                case PropertyDataType.Text:
                    return DataType.Text;
                case PropertyDataType.Integer:
                    return DataType.Integer;
                case PropertyDataType.Decimal:
                    return DataType.Decimal;
                case PropertyDataType.DateTime:
                    return DataType.DateTime;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        public static PropertyDto CreateDto(FieldDefinition subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new PropertyDto();
            result.Id = subject.Id;
            result.Version = subject.Version;
            result.Caption = subject.Name;
            result.DataType = ConvertDataType(subject.DataType);
            result.IsSystemProperty = subject.IsSystemField;

            return result;
        }

        public static PropertyDto[] CreateDtos(ICollection<FieldDefinition> subjects)
        {
            Guard.Against<ArgumentNullException>(subjects == null, "subjects");

            var result = new List<PropertyDto>();
            foreach (var each in subjects)
                result.Add(CreateDto(each));
            return result.ToArray();
        }

        public static FieldDefinition CreateDomainObject(PropertyDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new FieldDefinition();
            result.Name = subject.Caption;
            result.DataType = ConvertDataType(subject.DataType);
            return result;
        }


        public static FieldDefinition UpdateDomainObject(PropertyDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");
            var result = IoC.Resolve<IFieldDefinitionRepository>().Get(subject.Id);
            Guard.Against<EntityNotFoundException>(result == null, "Property entity not found");
            Guard.Against<DtoOutOfDateException>(result.Version != subject.Version, "Dto is out of date");

            result.Name = subject.Caption;
            result.DataType = ConvertDataType(subject.DataType);
            return result;
        }
    }
}