using System;
using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Assembler
{
    public class PropertyDefinitionAssembler
    {
        private static PropertyDtoDataType ConvertDataType(DomainPropertyType value)
        {
            switch (value)
            {
                case DomainPropertyType.Boolean:
                    return PropertyDtoDataType.Boolean;
                case DomainPropertyType.Text:
                    return PropertyDtoDataType.Text;
                case DomainPropertyType.Integer:
                    return PropertyDtoDataType.Integer;
                case DomainPropertyType.Decimal:
                    return PropertyDtoDataType.Decimal;
                case DomainPropertyType.DateTime:
                    return PropertyDtoDataType.DateTime;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        public static PropertyDto CreateDto(DomainPropertyDefinition subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new PropertyDto();
            result.Id = subject.Id;
            result.Caption = subject.Name;
            result.DataType = ConvertDataType(subject.DataType);

            return result;
        }

        public static PropertyDto[] CreateDtos(ICollection<DomainPropertyDefinition> subjects)
        {
            Guard.Against<ArgumentNullException>(subjects == null, "subjects");

            var result = new List<PropertyDto>();
            foreach (var each in subjects)
                result.Add(CreateDto(each));
            return result.ToArray();
        }
    }
}