using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.Models
{
    public class PropertyViewModel : ModelBase
    {
        private readonly IList<PropertyDataType> _propertyDataTypes;

        public PropertyViewModel() : this(new PropertyDto())
        {
        }

        public PropertyViewModel(PropertyDto subject)
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