namespace phiNdus.fundus.Web.Business.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BasePropertiesDto
    {
        private IList<FieldValueDto> _properties = new List<FieldValueDto>();
        public IList<FieldValueDto> Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }

        public void AddProperty(FieldValueDto subject)
        {
            _properties.Add(subject);
        }

        public void RemoveProperty(FieldValueDto subject)
        {
            _properties.Remove(subject);
        }

        public void RemoveProperty(int propertyId)
        {
            foreach (var each in _properties)
                if (each.PropertyId == propertyId)
                {
                    RemoveProperty(each);
                    break;
                }
        }

        protected FieldValueDto GetProperty(int propertyId)
        {
            return _properties.FirstOrDefault(each => each.PropertyId == propertyId);
        }

        public string GetPropertyValue(int propertyId)
        {
            var property = GetProperty(propertyId);
            return property != null ? Convert.ToString(property.Value) : null;
        }

        public bool IsDiscriminator(int propertyId)
        {
            var property = GetProperty(propertyId);
            return property != null ? property.IsDiscriminator : false;
        }
    }
}