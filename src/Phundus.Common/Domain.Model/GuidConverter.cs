namespace Phundus.Common.Domain.Model
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;

    public class GuidConverter<TId> : TypeConverter
    {
// ReSharper disable once StaticFieldInGenericType
        private static ConstructorInfo _ctor;

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {            
            if (sourceType == typeof (String) || sourceType == typeof (Guid))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var ctor = GetConstructor();
            if (value is string)
            {
                var guid = new Guid((string) value);

                return (TId) ctor.Invoke(new object[] {guid});
            }
            if (value is Guid)
            {
                return (TId) ctor.Invoke(new[] {value});
            }
            return base.ConvertFrom(context, culture, value);
        }

        private ConstructorInfo GetConstructor()
        {
            if (_ctor == null)
            {
                _ctor = typeof (TId).GetConstructor(new[] {typeof (Guid)});

                if (_ctor == null)
                    throw new Exception(
                        String.Format(
                            "Could not convert to {0}. The type has no public constructor with a Guid argument.",
                            typeof (TId).Name));
            }

            return _ctor;
        }
    }
}