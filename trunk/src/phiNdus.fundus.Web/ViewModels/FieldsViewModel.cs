namespace phiNdus.fundus.Web.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Practices.ServiceLocation;
    using Phundus.Core.Inventory._Legacy.Services;

    public class FieldsViewModel : ViewModelBase
    {
        private IList<FieldViewModel> _items = new List<FieldViewModel>();

        protected IFieldsService FieldsService
        {
            get { return ServiceLocator.Current.GetInstance<IFieldsService>(); }
        }

        public IList<FieldViewModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public void Save()
        {
            var dtos = Items.Select(each => each.CreateDto()).ToList();
            FieldsService.UpdateFields(dtos);
        }

        public FieldsViewModel Load()
        {
            Items.Clear();
            foreach (var each in FieldsService.GetProperties())
            {
                Items.Add(new FieldViewModel().Load(each));
            }
            return this;
        }
    }
}