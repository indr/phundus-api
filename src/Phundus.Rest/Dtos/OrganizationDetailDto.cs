namespace Phundus.Rest.Dtos
{
    using System;

    public class OrganizationDetailDto
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }

        public string EmailAddress { get; set; }
        public string Website { get; set; }
        public string Coordinate { get; set; }
        public string Startpage { get; set; }
        public DateTime CreateDate { get; set; }
        public string DocumentTemplate { get; set; }
    }
}