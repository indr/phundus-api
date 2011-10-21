using System;
using Iesi.Collections.Generic;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class FieldedEntity : Entity
    {
        private ISet<FieldValue> _fieldValues = new HashedSet<FieldValue>();

        public FieldedEntity()
        {
        }

        public FieldedEntity(ISet<FieldValue> fieldValues)
        {
            _fieldValues = fieldValues;
        }

        public FieldedEntity(int id, int version) : base(id, version)
        {
        }

        public virtual ISet<FieldValue> FieldValues
        {
            get { return _fieldValues; }
            protected set { _fieldValues = value; }
        }

        public virtual bool HasProperty(FieldDefinition propertyDefinition)
        {
            return HasProperty(propertyDefinition.Id);
        }

        public virtual bool HasProperty(int propertyDefinitionId)
        {
            foreach (var each in FieldValues)
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
            FieldValues.Add(result);
            return result;
        }

        public virtual object GetPropertyValue(FieldDefinition propertyDefinition)
        {
            return GetPropertyValue(propertyDefinition.Id);
        }

        public virtual object GetPropertyValue(int propertyDefinitionId)
        {
            foreach (var each in FieldValues)
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
            foreach (var each in FieldValues)
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
            foreach (var each in FieldValues)
                if (each.PropertyDefinition.Id == propertyDefinition.Id)
                {
                    propertyValue = each;
                    break;
                }
            if (propertyValue != null) {
                FieldValues.Remove(propertyValue);
            }
            else
                throw new PropertyException("Property nicht vorhanden.");
        }
    }
}