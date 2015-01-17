﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using DataAnnotationsExtensions;

namespace phiNdus.fundus.Web.Models
{
    public class LogOnModel
    {
        [Required]
        [DisplayName("E-Mail-Adresse")]
        [Email(ErrorMessage = "Ungültige E-Mail-Adresse")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Passwort")]
        public string Password { get; set; }

        [DisplayName("Angemeldet bleiben")]
        public bool RememberMe { get; set; }
    }


    // TODO: fundus-16
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public sealed class EmailAttribute : RegularExpressionAttribute
    {
        // http://weblogs.asp.net/scottgu/archive/2010/01/15/asp-net-mvc-2-model-validation.aspx
        public EmailAttribute()
            : base("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$")
        {
        }
    }
}