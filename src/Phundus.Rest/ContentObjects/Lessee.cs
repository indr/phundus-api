namespace Phundus.Rest.ContentObjects
{
    using AutoMapper;
    using Core.Shop.Queries;
    using Newtonsoft.Json;

    public class Lessee
    {
        static Lessee()
        {
            Mapper.CreateMap<OrderDto, Lessee>()
                .ForMember(d => d.LesseeId, o => o.MapFrom(s => s.Borrower_Id))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.Borrower_FirstName + " " + s.Borrower_LastName))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Borrower_Street))
                .ForMember(d => d.Postcode, o => o.MapFrom(s => s.Borrower_Postcode))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Borrower_City))
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.Borrower_EmailAddress))
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Borrower_MobilePhoneNumber))
                .ForMember(d => d.MemberNumber, o => o.MapFrom(s => s.Borrower_MemberNumber));
        }

        [JsonProperty("lesseeId")]
        public int LesseeId { get; set; }

        [JsonProperty("name")]
        public string FullName { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("memberNumber")]
        public string MemberNumber { get; set; }
    }
}