using System;
using System.Globalization;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class FieldValue : BaseEntity
    {
        private FieldDefinition _propertyDefinition;

        protected FieldValue()
        {
        }

        public FieldValue(FieldDefinition propertyDefinition) : this(propertyDefinition, null)
        {
        }

        public FieldValue(FieldDefinition propertyDefinition, object value)
        {
            _propertyDefinition = propertyDefinition;
            if (value != null)
                InternalSetValue(_propertyDefinition.DataType, value);
        }

        private object InternalGetValue(FieldType dataType)
        {
            switch (dataType)
            {
                case FieldType.Boolean:
                    return BooleanValue;
                case FieldType.Text:
                    return TextValue;
                case FieldType.Integer:
                    return IntegerValue;
                case FieldType.Decimal:
                    return DecimalValue;
                case FieldType.DateTime:
                    return DateTimeValue;
                default:
                    throw new ArgumentOutOfRangeException("dataType");
            }
        }

        private void InternalSetValue(FieldType dataType, object value)
        {
            switch (dataType)
            {
                case FieldType.Boolean:
                    if (value == null)
                        BooleanValue = null;
                    else
                        BooleanValue = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                    break;
                case FieldType.Text:
                    TextValue = value == null ? null : Convert.ToString(value, CultureInfo.InvariantCulture);
                    break;
                case FieldType.Integer:
                    if (value == null)
                        IntegerValue = null;
                    else
                        IntegerValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    break;
                case FieldType.Decimal:
                    if (value == null)
                        DecimalValue = null;
                    else
                        DecimalValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                    break;
                case FieldType.DateTime:
                    if (value == null)
                        DateTimeValue = null;
                    else
                        DateTimeValue = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("dataType");
            }
        }

        public virtual FieldDefinition PropertyDefinition
        {
            get { return _propertyDefinition; }
            protected set { _propertyDefinition = value; }
        }

        public virtual object Value
        {
            get { return InternalGetValue(PropertyDefinition.DataType); }
            set { InternalSetValue(PropertyDefinition.DataType, value); }
        }

        protected DateTime? DateTimeValue { get; set; }

        protected double? DecimalValue { get; set; }

        protected int? IntegerValue { get; set; }

        protected bool? BooleanValue { get; set; }

        protected string TextValue { get; set; }

        private bool _isDiscriminator;
        public virtual bool IsDiscriminator
        {
            get { return _isDiscriminator; }
            set
            {
                if (value)
                    Value = null;
                _isDiscriminator = value;
            }
        }
    }
}