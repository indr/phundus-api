using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace phiNdus.fundus.Web.ViewModels
{
    public class ValidationViewModel
    {
        [Required]
        [DisplayName("Code")]
        [StringLength(24, ErrorMessage = "Der Code muss 24 Zeichen enthalten.", MinimumLength = 24)]
        public string Key { get; set; }
    }
}