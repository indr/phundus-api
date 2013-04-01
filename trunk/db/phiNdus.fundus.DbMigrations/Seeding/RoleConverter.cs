using System;
using System.Globalization;
using CsvHelper.TypeConversion;

namespace phiNdus.fundus.DbMigrations
{
    public class RoleConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text)
        {
            return ConvertFromString(CultureInfo.InvariantCulture, text);
        }

        public override object ConvertFromString(CultureInfo culture, string text)
        {
            if (text == "Admin")
                return 3;
            if (text == "Chief")
                return 2;
            return 1;
        }

        public override bool CanConvertFrom(Type type)
        {
            return type == typeof (string);
        }
    }
}