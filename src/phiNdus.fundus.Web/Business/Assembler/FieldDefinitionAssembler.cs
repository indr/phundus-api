namespace phiNdus.fundus.Web.Business.Assembler
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;
    using fundus.Business;
    using phiNdus.fundus.Web.Business.Dto;
    using Phundus.Core;
    using Phundus.Core.InventoryCtx;
    using Phundus.Core.InventoryCtx.Model;
    using Phundus.Core.InventoryCtx.Repositories;
    using Phundus.Infrastructure;

    public class FieldDefinitionAssembler
    {
        private static FieldDataType ConvertDataType(DataType value)
        {
            switch (value)
            {
                case DataType.Boolean:
                    return FieldDataType.Boolean;
                case DataType.Text:
                    return FieldDataType.Text;
                case DataType.Integer:
                    return FieldDataType.Integer;
                case DataType.Decimal:
                    return FieldDataType.Decimal;
                case DataType.DateTime:
                    return FieldDataType.DateTime;
                case DataType.RichText:
                    return FieldDataType.RichText;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        private static DataType ConvertDataType(FieldDataType value)
        {
            switch (value)
            {
                case FieldDataType.Boolean:
                    return DataType.Boolean;
                case FieldDataType.Text:
                    return DataType.Text;
                case FieldDataType.Integer:
                    return DataType.Integer;
                case FieldDataType.Decimal:
                    return DataType.Decimal;
                case FieldDataType.DateTime:
                    return DataType.DateTime;
                case FieldDataType.RichText:
                    return DataType.RichText;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        public static FieldDefinitionDto CreateDto(FieldDefinition subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new FieldDefinitionDto();
            result.Id = subject.Id;
            result.Version = subject.Version;
            result.Caption = subject.Name;
            result.DataType = ConvertDataType(subject.DataType);
            result.IsSystem = subject.IsSystem;
            result.IsColumn = subject.IsColumn;
            result.IsDefault = subject.IsDefault;
            result.IsAttachable = subject.IsAttachable;
            result.Position = subject.Position;

            return result;
        }

        public static IList<FieldDefinitionDto> CreateDtos(ICollection<FieldDefinition> subjects)
        {
            Guard.Against<ArgumentNullException>(subjects == null, "subjects");

            var result = new List<FieldDefinitionDto>();
            foreach (var each in subjects)
                result.Add(CreateDto(each));
            return result;
        }

        public static FieldDefinition CreateDomainObject(FieldDefinitionDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new FieldDefinition();
            result.Name = subject.Caption;
            result.DataType = ConvertDataType(subject.DataType);
            result.Position = subject.Position;
            result.IsDefault = subject.IsDefault;
            result.IsColumn = subject.IsColumn;
            return result;
        }


        public static FieldDefinition UpdateDomainObject(FieldDefinitionDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");
            var result = ServiceLocator.Current.GetInstance<IFieldDefinitionRepository>().ById(subject.Id);
            Guard.Against<EntityNotFoundException>(result == null, "Property entity not found");
            Guard.Against<DtoOutOfDateException>(result.Version != subject.Version, "Dto is out of date");

            result.Name = subject.Caption;
            result.DataType = ConvertDataType(subject.DataType);
            result.IsDefault = subject.IsDefault;
            result.Position = subject.Position;
            return result;
        }
    }
}