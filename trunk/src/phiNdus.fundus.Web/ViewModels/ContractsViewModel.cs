using System.Collections.Generic;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class ContractsViewModel : ViewModelBase
    {
        private IList<ContractViewModel> _items = new List<ContractViewModel>();

        public IList<ContractViewModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }

    public class MyContractsViewModel : ContractsViewModel
    {
    }
}