﻿using System;

namespace phiNdus.fundus.Core.Web.Models
{
    public enum MessageBoxType
    {
        Error,
        Warning,
        Info,
        Success
    }

    public class MessageBoxViewModel
    {
        public string Message { get; set; }
        public MessageBoxType Type { get; set; }
    }
}