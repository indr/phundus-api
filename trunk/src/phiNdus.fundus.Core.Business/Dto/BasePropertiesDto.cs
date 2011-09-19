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
    }
}