using System;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class Setting : Entity
    {
        private double? _decimalValue;
        private int? _integerValue;
        private string _stringValue;

        public Setting()
        {
            
        }

        public Setting(string key)
        {
            _key = key;
        }

        private string _key;
        public virtual string Key
        {
            get { return _key; }
            protected set { _key = value; }
        }

        public virtual string StringValue
        {
            get { return _stringValue; }
            set
            {
                _decimalValue = null;
                _integerValue = null;
                _stringValue = value;
            }
        }

        public virtual int? IntegerValue
        {
            get { return _integerValue; }
            set
            {
                _stringValue = null;
                _decimalValue = null;
                _integerValue = value;
            }
        }

        public virtual double? DecimalValue
        {
            get { return _decimalValue; }
            set
            {
                _stringValue = null;
                _integerValue = null;
                _decimalValue = value;
            }
        }

        public virtual bool? BooleanValue
        {
            get
            {
                if (!IntegerValue.HasValue)
                    return null;
                return IntegerValue.Value == 1;
            }
            set
            {
                if (value.HasValue)
                    IntegerValue = value.Value ? 1 : 0;
                else
                    IntegerValue = null;
            }
        }
    }
}