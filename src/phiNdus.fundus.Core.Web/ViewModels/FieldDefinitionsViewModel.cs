using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class FieldDefinitionsViewModel
    {
        public IList<FieldDefinitionDto> Items { get; set; }
    }
}