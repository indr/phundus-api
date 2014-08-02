using System;

namespace phiNdus.fundus.Web.ViewModels
{
    using Phundus.Core.Inventory._Legacy.Dtos;

    public class PropertyValueViewModel
    {
        public string Caption { get; set; }
        public FieldDataType DataType { get; set; }
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

        public bool IsCalculated { get; set; }

        public int Position { get; set; }
    }
}