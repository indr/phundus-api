using System;
using System.Globalization;

namespace phiNdus.fundus.Domain.Entities
{
    public class FieldValue : EntityBase
    {
        private FieldDefinition _fieldDefinition;

        protected FieldValue()
        {
        }

        public FieldValue(FieldDefinition fieldDefinition) : this(fieldDefinition, null)
        {
        }

        public FieldValue(FieldDefinition fieldDefinition, object value)
        {
            _fieldDefinition = fieldDefinition;
            if (value != null)
                InternalSetValue(_fieldDefinition.DataType, value);
        }

        private object InternalGetValue(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Boolean:
                    return BooleanValue;
                case DataType.Text:
                    return TextValue;
                case DataType.Integer:
                    return IntegerValue;
                case DataType.Decimal:
                    return DecimalValue;
                case DataType.DateTime:
                    return DateTimeValue;
                case DataType.RichText:
                    return TextValue;
                default:
                    throw new ArgumentOutOfRangeException("dataType");
            }
        }

        private void InternalSetValue(DataType dataType, object value)
        {
            switch (dataType)
            {
                case DataType.Boolean:
                    if (value == null)
                        BooleanValue = null;
                    else
                        BooleanValue = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                    break;
                case DataType.Text:
                    TextValue = value == null ? null : Convert.ToString(value, CultureInfo.InvariantCulture);
                    break;
                case DataType.Integer:
                    if (value == null)
                        IntegerValue = null;
                    else
                        IntegerValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    break;
                case DataType.Decimal:
                    if (value == null)
                        DecimalValue = null;
                    else
                        DecimalValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                    break;
                case DataType.DateTime:
                    if (value == null)
                        DateTimeValue = null;
                    else
                        DateTimeValue = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                    break;
                case DataType.RichText:
                    TextValue = value == null ? null : Convert.ToString(value, CultureInfo.InvariantCulture);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("dataType");
            }
        }

        public virtual FieldDefinition FieldDefinition
        {
            get { return _fieldDefinition; }
            protected set { _fieldDefinition = value; }
        }

        public virtual object Value
        {
            get { return InternalGetValue(FieldDefinition.DataType); }
            set { InternalSetValue(FieldDefinition.DataType, value); }
        }

        protected DateTime? DateTimeValue { get; set; }

        protected double? DecimalValue { get; set; }

        protected int? IntegerValue { get; set; }

        protected bool? BooleanValue { get; set; }

        public virtual string TextValue { get; set; }

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