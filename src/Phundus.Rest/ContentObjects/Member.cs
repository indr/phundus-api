namespace Phundus.Rest.ContentObjects
{
    using System;
    using Newtonsoft.Json;

    public class Member
    {
        [JsonProperty("memberId")]
        public Guid Guid { get; set; }
        public int MemberVersion { get; set; }
        public int MembershipVersion { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? JsNumber { get; set; }
        public string EmailAddress { get; set; }

        public int Role { get; set; }
        public bool IsManager { get; set; }

        public DateTime? RequestDate { get; set; }

        public bool IsLocked { get; set; }

        public DateTime? ApprovalDate { get; set; }
        public string FullName { get; set; }
    }
}