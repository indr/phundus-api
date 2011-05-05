using System;

namespace phiNdus.fundus.Core.Business {

    public class UserDto {
        public string Mail { get; set; }

        public string PasswordQuestion { get; set; }

        public bool Approved { get; set; }
        public DateTime CreationDate { get; set; }
    }
}