namespace Phundus.Common.Infrastructure.Persistence
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