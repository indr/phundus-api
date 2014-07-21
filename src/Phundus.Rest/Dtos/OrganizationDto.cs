namespace Phundus.Rest.Dtos
{
    using System;

    public class OrganizationDto : OrganizationListDto
    {
        public string EmailAddress { get; set; }
        public string Website { get; set; }
        public string Coordinate { get; set; }
        public string Startpage { get; set; }
        public DateTime CreateDate { get; set; }
        public string DocumentTemplate { get; set; }
    }
}