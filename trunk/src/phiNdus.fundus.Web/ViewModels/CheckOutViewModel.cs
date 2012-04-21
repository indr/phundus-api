using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Web.Models;
using Rhino.Commons;

namespace phiNdus.fundus.Web.ViewModels
{
    public class CheckOutViewModel : ViewModelBase
    {
        public CheckOutViewModel() : base()
        {
            Cart = new CartViewModel();
            Customer = new UserModel();
        }


        public CartViewModel Cart { get; set; }
        public UserModel Customer { get; set; }
    }
}