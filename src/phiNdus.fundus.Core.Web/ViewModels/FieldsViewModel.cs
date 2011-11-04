using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Business.Security.Constraints;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class FieldsViewModel : ViewModelBase
    {
        public FieldsViewModel()
        {
            Items = FieldsService.GetProperties(HttpContext.Current.Session.SessionID);
        }

        protected IFieldsService FieldsService
        {
            get { return IoC.Resolve<IFieldsService>(); }
        }

        public IList<FieldDefinitionDto> Items { get; set; }

        public void Save()
        {
            FieldsService.UpdateFields(SessionId, Items);
        }
    }
}