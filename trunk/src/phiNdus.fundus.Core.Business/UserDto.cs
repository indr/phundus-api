using System;

namespace phiNdus.fundus.Core.Business {

    public class UserDto {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PasswordQuestion { get; set; }

        public bool IsApproved { get; set; }
        // Todo: Enzi hat vorgeschlagen, Zeitstempel als Strings (UTC?) zu übermitteln.
        public DateTime CreateDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}