using System;
using System.Collections.Generic;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class FieldDefinitionViewModel
    {
        private readonly IList<FieldDataType> _fieldDataTypes;

        public FieldDefinitionViewModel() : this(new FieldDefinitionDto())
        {
        }

        public FieldDefinitionViewModel(FieldDefinitionDto subject)
        {
            Item = subject;
            _fieldDataTypes = new List<FieldDataType>
                                     {
                                         FieldDataType.Boolean,
                                         FieldDataType.DateTime,
                                         FieldDataType.Decimal,
                                         FieldDataType.Integer,
                                         FieldDataType.Text
                                     };
        }


        public FieldDefinitionDto Item { get; set; }


        public IEnumerable<SelectListItem> FieldDataTypes
        {
            get
            {
                var result = new List<SelectListItem>();
                foreach (var each in _fieldDataTypes)
                {
                    var item = new SelectListItem();
                    item.Value = Convert.ToString(Convert.ToInt32(each));
                    item.Text = Convert.ToString(each);
                    item.Selected = (Convert.ToString(each) == Convert.ToString(Item.DataType));
                    result.Add(item);
                }
                return result;
                // TODO,Inder: Hilfe, ich kann Selected nicht setzen?!
                //return _fieldDataTypes.Select(each => new SelectListItem
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