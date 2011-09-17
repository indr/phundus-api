using System;
using Iesi.Collections.Generic;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class BasePropertyEntity : BaseEntity
    {
        private ISet<DomainPropertyValue> _propertyValues = new HashedSet<DomainPropertyValue>();

        public BasePropertyEntity()
        {
        }

        public BasePropertyEntity(ISet<DomainPropertyValue> propertyValues)
        {
            PropertyValues = propertyValues;
        }

        protected ISet<DomainPropertyValue> PropertyValues
        {
            get { return _propertyValues; }
            set { _propertyValues = value; }
        }

        public virtual bool HasProperty(DomainProperty property)
        {
            foreach (var each in PropertyValues)
            {
                if (each.Property.Id == property.Id)
                    return true;
            }
            return false;
        }

        public virtual void AddProperty(DomainProperty property)
        {
            if (HasProperty(property))
                throw new Exception("Property bereits vorhanden.");

            PropertyValues.Add(new DomainPropertyValue(property));
        }

        public virtual object GetPropertyValue(DomainProperty property)
        {
            foreach (var each in PropertyValues)
            {
                if (each.Property.Id == property.Id)
                    return each.Value;
            }
            throw new Exception("Property nicht vorhanden.");
        }

        public virtual void SetPropertyValue(DomainProperty property, object value)
        {
            foreach (var each in PropertyValues)
            {
                if (each.Property.Id == property.Id)
                {
                    each.Value = value;
                    return;
                }
            }
            throw new Exception("Property nicht vorhanden.");
        }

        public virtual void RemoveProperty(DomainProperty property)
        {
            DomainPropertyValue propertyValue = null;
            foreach (var each in PropertyValues)
                if (each.Property.Id == property.Id)
                {
                    propertyValue = each;
                    break;
                }
            if (propertyValue != null)
                PropertyValues.Remove(propertyValue);
            else
                throw new Exception("Property nicht vorhanden.");
        }
    }
}