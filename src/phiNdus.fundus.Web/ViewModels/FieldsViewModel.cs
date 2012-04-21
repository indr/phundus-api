using System.Collections.Generic;
using System.Linq;
using phiNdus.fundus.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Web.ViewModels
{
    public class FieldsViewModel : ViewModelBase
    {
        private IList<FieldViewModel> _items = new List<FieldViewModel>();

        protected IFieldsService FieldsService
        {
            get { return IoC.Resolve<IFieldsService>(); }
        }

        public IList<FieldViewModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public void Save()
        {
            var dtos = Items.Select(each => each.CreateDto()).ToList();
            FieldsService.UpdateFields(SessionId, dtos);
        }

        public FieldsViewModel Load()
        {
            Items.Clear();
            foreach (var each in FieldsService.GetProperties(SessionId))
            {
                Items.Add(new FieldViewModel().Load(each));
            }
            return this;
        }
    }
}