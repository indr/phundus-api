namespace Phundus.Core.InventoryCtx.Model
{
    using System;
    using System.Linq;
    using Ddd;
    using Exceptions;
    using Iesi.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;
    using Repositories;

    /// <summary>
    /// Die Klasse FieldedEntity stellt Funktionen für dynamische Felder zur Verfügung.
    /// </summary>
    public class FieldedEntity : EntityBase
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

        public virtual FieldValue AddField(FieldDefinition fieldDefinition)
        {
            if (fieldDefinition == null)
                throw new ArgumentNullException("fieldDefinition");
            return AddField(fieldDefinition, null);
        }

        public virtual FieldValue AddField(FieldDefinition fieldDefinition, object value)
        {
            if (fieldDefinition == null)
                throw new ArgumentNullException("fieldDefinition");

            if (HasField(fieldDefinition))
                throw new FieldAlreadyAttachedException("Property bereits vorhanden.");

            var result = new FieldValue(fieldDefinition, value);
            FieldValues.Add(result);
            return result;
        }

        public virtual object GetFieldValue(FieldDefinition fieldDefinition)
        {
            return GetFieldValue(fieldDefinition.Id);
        }

        public virtual object GetFieldValue(int fieldDefinitionId)
        {
            foreach (var each in FieldValues.Where(each => each.FieldDefinition.Id == fieldDefinitionId))
            {
                return each.Value;
            }
            throw new FieldAlreadyAttachedException("Property nicht vorhanden.");
        }

        protected virtual int GetFieldValueAsInt32(int fieldDefinitionId, int defaultValue)
        {
            return !HasField(fieldDefinitionId)
                       ? defaultValue
                       : Convert.ToInt32(GetFieldValue(fieldDefinitionId));
        }

        protected bool GetFieldValueAsBoolean(int fieldDefinitionId, bool defaultValue)
        {
            return !HasField(fieldDefinitionId)
                       ? defaultValue
                       : Convert.ToBoolean(GetFieldValue(fieldDefinitionId));
        }

        protected string GetFieldValueAsString(int fieldDefinitionId, string defaultValue)
        {
            return !HasField(fieldDefinitionId)
                       ? defaultValue
                       : Convert.ToString(GetFieldValue(fieldDefinitionId));
        }

        protected double GetFieldValueAsDouble(int fieldDefinitionId, double defaultValue)
        {
            return !HasField(fieldDefinitionId)
                       ? defaultValue
                       : Convert.ToDouble(GetFieldValue(fieldDefinitionId));
        }


        public virtual bool HasField(FieldDefinition fieldDefinition)
        {
            return HasField(fieldDefinition.Id);
        }

        public virtual bool HasField(int fieldDefinitionId)
        {
            return FieldValues.Any(each => each.FieldDefinition.Id == fieldDefinitionId);
        }

        public virtual void RemoveField(FieldDefinition fieldDefinition)
        {
            var fieldValue = FieldValues.FirstOrDefault(each => each.FieldDefinition.Id == fieldDefinition.Id);
            if (fieldValue != null)
            {
                FieldValues.Remove(fieldValue);
            }
            else
                throw new FieldAlreadyAttachedException("Property nicht vorhanden.");
        }

        public virtual FieldValue SetFieldValue(FieldDefinition fieldDefinition, object value)
        {
            return SetFieldValue(fieldDefinition.Id, value);
        }

        public virtual FieldValue SetFieldValue(int fieldDefinitionId, object value)
        {
            return SetFieldValue(fieldDefinitionId, value, false);
        }

        public virtual FieldValue SetFieldValue(int fieldDefinitionId, object value, bool attachIfNotExists)
        {
            if (attachIfNotExists && !HasField(fieldDefinitionId))
                AddField(ServiceLocator.Current.GetInstance<IFieldDefinitionRepository>().ById(fieldDefinitionId));
            foreach (var each in FieldValues.Where(each => each.FieldDefinition.Id == fieldDefinitionId))
            {
                each.Value = value;
                return each;
            }
            throw new FieldAlreadyAttachedException("Property nicht vorhanden.");
        }
    }
}