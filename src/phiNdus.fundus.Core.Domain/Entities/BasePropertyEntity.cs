using System;
using Iesi.Collections.Generic;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class BasePropertyEntity : BaseEntity
    {
        private ISet<FieldValue> _propertyValues = new HashedSet<FieldValue>();

        public BasePropertyEntity()
        {
        }

        public BasePropertyEntity(ISet<FieldValue> propertyValues)
        {
            _propertyValues = propertyValues;
        }

        public BasePropertyEntity(int id, int version) : base(id, version)
        {
        }

        public virtual ISet<FieldValue> PropertyValues
        {
            get { return _propertyValues; }
            protected set { _propertyValues = value; }
        }

        public virtual bool HasProperty(FieldDefinition propertyDefinition)
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

        public virtual FieldValue AddProperty(FieldDefinition propertyDefinition)
        {
            return AddProperty(propertyDefinition, null);
        }

        public virtual FieldValue AddProperty(FieldDefinition propertyDefinition, object value)
        {
            if (HasProperty(propertyDefinition))
                throw new PropertyException("Property bereits vorhanden.");

            var result = new FieldValue(propertyDefinition, value);
            PropertyValues.Add(result);
            return result;
        }

        public virtual object GetPropertyValue(FieldDefinition propertyDefinition)
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

        public virtual FieldValue SetPropertyValue(FieldDefinition propertyDefinition, object value)
        {
            return SetPropertyValue(propertyDefinition.Id, value);
        }

        public virtual FieldValue SetPropertyValue(int propertyDefinitionId, object value)
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

        public virtual void RemoveProperty(FieldDefinition propertyDefinition)
        {
            FieldValue propertyValue = null;
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