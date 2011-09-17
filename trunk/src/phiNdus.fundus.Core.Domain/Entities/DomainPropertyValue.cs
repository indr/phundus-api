using System;
using System.Globalization;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class DomainPropertyValue : BaseEntity
    {
        protected DomainPropertyValue() : base()
        {
        }

        public DomainPropertyValue(DomainPropertyDefinition propertyDefinition) : base()
        {
            _propertyDefinition = propertyDefinition;
        }

        private DomainPropertyDefinition _propertyDefinition;
        public virtual DomainPropertyDefinition PropertyDefinition
        {
            get { return _propertyDefinition; }
            protected set { _propertyDefinition = value; }
        }

        public virtual object Value
        {
            get
            {
                switch (PropertyDefinition.DataType)
                {
                    case DomainPropertyType.Boolean:
                        return BooleanValue;
                    case DomainPropertyType.Text:
                        return TextValue;
                    case DomainPropertyType.Integer:
                        return IntegerValue;
                    case DomainPropertyType.Decimal:
                        return DecimalValue;
                    case DomainPropertyType.DateTime:
                        return DateTimeValue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (PropertyDefinition.DataType)
                {
                    case DomainPropertyType.Boolean:
                        BooleanValue = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                        break;
                    case DomainPropertyType.Text:
                        TextValue = Convert.ToString(value, CultureInfo.InvariantCulture);
                        break;
                    case DomainPropertyType.Integer:
                        IntegerValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                        break;
                    case DomainPropertyType.Decimal:
                        DecimalValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                        break;
                    case DomainPropertyType.DateTime:
                        DateTimeValue = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected DateTime? DateTimeValue { get; set; }

        protected double? DecimalValue { get; set; }

        protected int? IntegerValue { get; set; }

        protected bool? BooleanValue { get; set; }

        protected string TextValue { get; set; }
    }
}