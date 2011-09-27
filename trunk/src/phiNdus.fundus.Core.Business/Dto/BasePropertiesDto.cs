﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace phiNdus.fundus.Core.Business.Dto
{
    public class BasePropertiesDto
    {
        private IList<DtoProperty> _properties = new List<DtoProperty>();
        public IList<DtoProperty> Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }

        public void AddProperty(DtoProperty subject)
        {
            _properties.Add(subject);
        }

        public void RemoveProperty(DtoProperty subject)
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

        protected DtoProperty GetProperty(int propertyId)
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