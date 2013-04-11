using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using phiNdus.fundus.Web.Models;
using phiNdus.fundus.Web.Models.CartModels;

namespace phiNdus.fundus.Web.ViewModels
{
    public class CheckOutViewModel : ViewModelBase
    {
        public CartModel Cart { get; set; }
        public UserModel Customer { get; set; }
    }
}