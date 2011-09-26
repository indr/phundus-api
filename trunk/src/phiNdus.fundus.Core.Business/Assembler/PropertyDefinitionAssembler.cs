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
        private static PropertyDataType ConvertDataType(DomainPropertyType value)
        {
            switch (value)
            {
                case DomainPropertyType.Boolean:
                    return PropertyDataType.Boolean;
                case DomainPropertyType.Text:
                    return PropertyDataType.Text;
                case DomainPropertyType.Integer:
                    return PropertyDataType.Integer;
                case DomainPropertyType.Decimal:
                    return PropertyDataType.Decimal;
                case DomainPropertyType.DateTime:
                    return PropertyDataType.DateTime;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        private static DomainPropertyType ConvertDataType(PropertyDataType value)
        {
            switch (value)
            {
                case PropertyDataType.Boolean:
                    return DomainPropertyType.Boolean;
                case PropertyDataType.Text:
                    return DomainPropertyType.Text;
                case PropertyDataType.Integer:
                    return DomainPropertyType.Integer;
                case PropertyDataType.Decimal:
                    return DomainPropertyType.Decimal;
                case PropertyDataType.DateTime:
                    return DomainPropertyType.DateTime;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        public static PropertyDto CreateDto(DomainPropertyDefinition subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new PropertyDto();
            result.Id = subject.Id;
            result.Version = subject.Version;
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

        public static DomainPropertyDefinition CreateDomainObject(PropertyDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new DomainPropertyDefinition();
            result.Name = subject.Caption;
            result.DataType = ConvertDataType(subject.DataType);
            return result;
        }


        public static DomainPropertyDefinition UpdateDomainObject(PropertyDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");
            var result = IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(subject.Id);
            Guard.Against<EntityNotFoundException>(result == null, "Property entity not found");
            Guard.Against<DtoOutOfDateException>(result.Version != subject.Version, "Dto is out of date");

            result.Name = subject.Caption;
            result.DataType = ConvertDataType(subject.DataType);
            return result;
        }
    }
}