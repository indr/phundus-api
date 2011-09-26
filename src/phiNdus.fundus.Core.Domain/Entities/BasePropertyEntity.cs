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
            _propertyValues = propertyValues;
        }

        public BasePropertyEntity(int id, int version) : base(id, version)
        {
        }

        public virtual ISet<DomainPropertyValue> PropertyValues
        {
            get { return _propertyValues; }
            protected set { _propertyValues = value; }
        }

        public virtual bool HasProperty(DomainPropertyDefinition propertyDefinition)
        {
            return HasProperty(propertyDefinition.Id);
        }

        public virtual bool HasProperty(int propertyDefinitionId)
        {
            foreach (var each in PropertyValues)
            {
                if (each.PropertyDefinition.Id == propertyDefinitionId)
                    return true;
            }
            return false;
        }

        public virtual DomainPropertyValue AddProperty(DomainPropertyDefinition propertyDefinition)
        {
            return AddProperty(propertyDefinition, null);
        }

        public virtual DomainPropertyValue AddProperty(DomainPropertyDefinition propertyDefinition, object value)
        {
            if (HasProperty(propertyDefinition))
                throw new PropertyException("Property bereits vorhanden.");

            var result = new DomainPropertyValue(propertyDefinition, value);
            PropertyValues.Add(result);
            return result;
        }

        public virtual object GetPropertyValue(DomainPropertyDefinition propertyDefinition)
        {
            return GetPropertyValue(propertyDefinition.Id);
        }

        public virtual object GetPropertyValue(int propertyDefinitionId)
        {
            foreach (var each in PropertyValues)
            {
                if (each.PropertyDefinition.Id == propertyDefinitionId)
                    return each.Value;
            }
            throw new PropertyException("Property nicht vorhanden.");
        }

        public virtual DomainPropertyValue SetPropertyValue(DomainPropertyDefinition propertyDefinition, object value)
        {
            return SetPropertyValue(propertyDefinition.Id, value);
        }

        public virtual DomainPropertyValue SetPropertyValue(int propertyDefinitionId, object value)
        {
            foreach (var each in PropertyValues)
            {
                if (each.PropertyDefinition.Id == propertyDefinitionId)
                {
                    each.Value = value;
                    return each;
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
            if (propertyValue != null) {
                PropertyValues.Remove(propertyValue);
            }
            else
                throw new PropertyException("Property nicht vorhanden.");
        }
    }
}