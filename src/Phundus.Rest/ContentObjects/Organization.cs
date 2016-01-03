namespace Phundus.Rest.ContentObjects
{
    using System;
    using AutoMapper;
    using Core.IdentityAndAccess.Queries;
    using Newtonsoft.Json;

    public class Organization
    {
        static Organization()
        {
            Mapper.CreateMap<OrganizationDto, Organization>()
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.Guid))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url));
        }

        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}