using System;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class DomainPropertyValue : BaseEntity
    {
        public DomainPropertyValue(DomainProperty property)
        {
            Property = property;
        }

        public DomainProperty Property { get; protected set; }

        public object Value
        {
            get
            {
                switch (Property.Type)
                {
#pragma warning disable 162 // Unreachable code. Siehe "break".
                    case DomainPropertyType.Boolean:
                        return BooleanValue;
                        break;
                    case DomainPropertyType.Text:
                        return TextValue;
                        break;
                    case DomainPropertyType.Integer:
                        return IntegerValue;
                        break;
                    case DomainPropertyType.Decimal:
                        return DecimalValue;
                        break;
                    case DomainPropertyType.DateTime:
                        return DateTimeValue;
                        break;
#pragma warning restore 162
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (Property.Type)
                {
                    case DomainPropertyType.Boolean:
                        BooleanValue = Convert.ToBoolean(value);
                        break;
                    case DomainPropertyType.Text:
                        TextValue = Convert.ToString(value);
                        break;
                    case DomainPropertyType.Integer:
                        IntegerValue = Convert.ToInt32(value);
                        break;
                    case DomainPropertyType.Decimal:
                        DecimalValue = Convert.ToDouble(value);
                        break;
                    case DomainPropertyType.DateTime:
                        DateTimeValue = Convert.ToDateTime(value);
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