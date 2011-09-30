﻿using System;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.Models
{
    public class PropertyValueViewModel : ModelBase
    {
        public string Caption { get; set; }
        public PropertyDataType DataType { get; set; }
        public int PropertyDefinitionId { get; set; }
        public int PropertyValueId { get; set; }
        public object Value { get; set; }
        public string StringValue
        {
            get { return Convert.ToString(Value); }
            set { Value = value; }
        }
        public bool BooleanValue
        {
            get { return Convert.ToBoolean(Value); }
            set { Value = value; }
        }
    }

    //public class PropertyValueViewModelEx<T> : PropertyValueViewModel
    //{
    //    //public T Value { get; set; }
    //}
}