using System;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class PropertyValueViewModel
    {
        public string Caption { get; set; }
        public PropertyDataType DataType { get; set; }
        public int PropertyDefinitionId { get; set; }
        public int PropertyValueId { get; set; }
        public object Value { get; set; }
        public bool IsDeleted { get; set; }
        public string StringValue
        {
            get { return Convert.ToString(Value); }
            set { Value = value; }
        }
        public bool BooleanValue
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(Value);
                }
                catch
                {
                    return false;
                }
            }
            set { Value = value; }
        }
    }

    //public class PropertyValueViewModelEx<T> : PropertyValueViewModel
    //{
    //    //public T Value { get; set; }
    //}
}