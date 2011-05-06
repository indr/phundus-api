using System;

namespace phiNdus.fundus.Core.Business {

    public class UserDto {
        public string Mail { get; set; }

        public string PasswordQuestion { get; set; }

        public bool Approved { get; set; }
        // Todo: Enzi hat vorgeschlagen, Zeitstempel als Strings (UTC?) zu übermitteln.
        public DateTime CreationDate { get; set; }
    }
}