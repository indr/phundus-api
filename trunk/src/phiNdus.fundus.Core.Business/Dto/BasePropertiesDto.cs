using System;
using System.Collections.Generic;

namespace phiNdus.fundus.Core.Business.Dto
{
    public class BasePropertiesDto
    {
        private ICollection<DtoProperty> _properties = new List<DtoProperty>();
        public ICollection<DtoProperty> Properties
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
    }
}