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

        public bool IsDiscriminator { get; set; }

        private object _value;
        public object Value
        {
            get { return _value; }
            set
            {
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

        public PropertyDataType DataType { get; set; }
    }
}