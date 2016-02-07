namespace Phundus.Persistence.Extensions
{
    using FluentNHibernate.Mapping;

    public static class FluentMappingExtensions
    {
        public static PropertyPart WithMaxSize(this PropertyPart propertyPart)
        {
            return propertyPart.Length(10000);
        }
    }
}