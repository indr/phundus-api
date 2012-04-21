using System;

namespace phiNdus.fundus.Core.Business.Dto
{
    public class FieldValueDto
    {
        /// <summary>
        /// DomainPropertyValueId
        /// </summary>
        public int ValueId { get; set; }

        /// <summary>
        /// DomainPropertyDefinitionId
        /// </summary>
        public int PropertyId { get; set; }

        public bool IsDiscriminator { get; set; }

        private object _value;
        public object Value
        {
            get { return _value; }
            set
            {
                IsDiscriminator = false;
                // TODO: UpdateModel() übergibt ein Array?
                if (value is Array)
                {
                    var array = (Array) value;
                    _value = array.GetValue(0);
                }
                else
                    _value = value;
            }
        }

        public bool ValueAsBoolean
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(Value);
                }
                catch
                {
                    return false;
                }
            }
            set { Value = value; }
        }

        public string ValueAsString
        {
            get { return Convert.ToString(Value);  }
            set { Value = value; }
        }

        public string Caption { get; set; }

        public FieldDataType DataType { get; set; }

        public bool IsCalculated { get; set; }

        public int Position { get; set; }
    }
}