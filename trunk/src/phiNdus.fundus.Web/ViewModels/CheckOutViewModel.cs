namespace phiNdus.fundus.Web.ViewModels
{
    using Models;
    using Models.CartModels;

    public class CheckOutViewModel : ViewModelBase
    {
        public CartModel Cart { get; set; }
        public UserModel Customer { get; set; }
    }
}