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

        public virtual bool HasProperty(DomainPropertyDefinition propertyDefinition)
        {
            foreach (var each in PropertyValues)
            {
                if (each.PropertyDefinition.Id == propertyDefinition.Id)
                    return true;
            }
            return false;
        }

        public virtual void AddProperty(DomainPropertyDefinition propertyDefinition)
        {
            if (HasProperty(propertyDefinition))
                throw new PropertyException("Property bereits vorhanden.");

            PropertyValues.Add(new DomainPropertyValue(propertyDefinition));
        }

        public virtual object GetPropertyValue(DomainPropertyDefinition propertyDefinition)
        {
            foreach (var each in PropertyValues)
            {
                if (each.PropertyDefinition.Id == propertyDefinition.Id)
                    return each.Value;
            }
            throw new PropertyException("Property nicht vorhanden.");
        }

        public virtual void SetPropertyValue(DomainPropertyDefinition propertyDefinition, object value)
        {
            foreach (var each in PropertyValues)
            {
                if (each.PropertyDefinition.Id == propertyDefinition.Id)
                {
                    each.Value = value;
                    return;
                }
            }
            throw new PropertyException("Property nicht vorhanden.");
        }

        public virtual void RemoveProperty(DomainPropertyDefinition propertyDefinition)
        {
            DomainPropertyValue propertyValue = null;
            foreach (var each in PropertyValues)
                if (each.PropertyDefinition.Id == propertyDefinition.Id)
                {
                    propertyValue = each;
                    break;
                }
            if (propertyValue != null)
                PropertyValues.Remove(propertyValue);
            else
                throw new PropertyException("Property nicht vorhanden.");
        }
    }
}