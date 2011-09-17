using System;
using System.Collections.Generic;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class BasePropertyEntity : BaseEntity
    {
        private ICollection<DomainPropertyValue> _properties = new List<DomainPropertyValue>();

        public BasePropertyEntity() : base()
        {
            
        }

        public BasePropertyEntity(ICollection<DomainPropertyValue> properties) : base()
        {
            _properties = properties;
        }

        public bool HasProperty(DomainProperty property)
        {
            foreach (var each in _properties)
            {
                if (each.Property == property)
                    return true;
            }
            return false;
        }

        public void AddProperty(DomainProperty property)
        {
            _properties.Add(new DomainPropertyValue(property));
        }

        public object GetPropertyValue(DomainProperty property)
        {
            foreach (var each in _properties)
            {
                if (each.Property == property)
                    return each.Value;
            }
            throw new Exception("Property nicht vorhanden.");
        }

        public void SetPropertyValue(DomainProperty property, object value)
        {
            foreach (var each in _properties)
                if (each.Property == property)
                {
                    each.Value = value;
                    return;
                }

            throw new Exception("Property nicht vorhanden.");
                    
        }
    }
}