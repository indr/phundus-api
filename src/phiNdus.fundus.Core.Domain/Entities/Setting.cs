using System;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class Setting : BaseEntity
    {
        public virtual string Key { get; protected set; }

        private string _string;
        public virtual string String
        {
            get { return _string; }
            set
            {
                _decimal = null;
                _integer = null;
                _string = value;
            }
        }

        private int? _integer;
        public virtual int? Integer
        {
            get { return _integer; }
            set
            {
                _string = null;
                _decimal = null;
                _integer = value;
            }
        }

        private double? _decimal;
        public virtual double? Decimal
        {
            get { return _decimal; }
            set
            {
                _string = null;
                _integer = null;
                _decimal = value;
            }
        }

        public virtual bool? Boolean
        {
            get
            {
                if (!Integer.HasValue)
                    return null;
                return Integer.Value == 1;
            }
            set
            {
                if (value.HasValue)
                    Integer = value.Value ? 1 : 0;
                else
                    Integer = null;
            }
        }
    }
}