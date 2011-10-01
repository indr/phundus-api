using System;
using System.Collections.Generic;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class PropertyDefinitionViewModel
    {
        private readonly IList<PropertyDataType> _propertyDataTypes;

        public PropertyDefinitionViewModel() : this(new PropertyDto())
        {
        }

        public PropertyDefinitionViewModel(PropertyDto subject)
        {
            Item = subject;
            _propertyDataTypes = new List<PropertyDataType>
                                     {
                                         PropertyDataType.Boolean,
                                         PropertyDataType.DateTime,
                                         PropertyDataType.Decimal,
                                         PropertyDataType.Integer,
                                         PropertyDataType.Text
                                     };
        }


        public PropertyDto Item { get; set; }


        public IEnumerable<SelectListItem> PropertyDataTypes
        {
            get
            {
                var result = new List<SelectListItem>();
                foreach (var each in _propertyDataTypes)
                {
                    var item = new SelectListItem();
                    item.Value = Convert.ToString(Convert.ToInt32(each));
                    item.Text = Convert.ToString(each);
                    item.Selected = (Convert.ToString(each) == Convert.ToString(Item.DataType));
                    result.Add(item);
                }
                return result;
                // TODO,Inder: Hilfe, ich kann Selected nicht setzen?!
                //return _propertyDataTypes.Select(each => new SelectListItem
                //                                             {
                //                                                 Value = Convert.ToString(Convert.ToInt32(each)),
                //                                                 Text = Convert.ToString(each),
                //                                                 Selected =
                //                                                     (Convert.ToString(each) ==
                //                                                      Convert.ToString(Item.DataType))
                //                                             });
            }
        }
    }
}