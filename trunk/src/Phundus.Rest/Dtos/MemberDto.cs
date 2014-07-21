namespace Phundus.Rest.Dtos
{
    using System;

    public class MemberDto
    {
        public int Id { get; set; }
        public int MemberVersion { get; set; }
        public int MembershipVersion { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? JsNumber { get; set; }
        public string EmailAddress { get; set; }

        public int Role { get; set; }

        public DateTime? RequestDate { get; set; }

        public bool IsLockedOut { get; set; }
        public DateTime? LastLockoutDate { get; set; }

        public bool IsApproved { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}