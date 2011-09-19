using System;

namespace phiNdus.fundus.Core.Business.Dto
{
    public class DtoProperty
    {
        /// <summary>
        /// DomainPropertyValueId
        /// </summary>
        public int ValueId { get; set; }

        /// <summary>
        /// DomainPropertyDefinitionId
        /// </summary>
        public int PropertyId { get; set; }

        public object Value { get; set; }

        public string Caption { get; set; }

        public DtoPropertyDataType DataType { get; set; }
    }

    public enum DtoPropertyDataType
    {
        Boolean,
        Text,
        Integer,
        Decimal,
        DateTime
    }
}