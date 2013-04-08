namespace phiNdus.fundus.Web.Models.Organization
{
    using Domain.Entities;

    public class OrganizationModel
    {
        public OrganizationModel(Organization organization)
        {
            Id = organization.Id;
            Name = organization.Name;
            Address = organization.Address;
            Coordinate = organization.Coordinate;
            Startpage = organization.Startpage;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Coordinate { get; set; }

        public string Startpage { get; set; }

        public bool HasOptionJoin { get; set; }

        public bool HasOptionLeave { get; set; }

        public bool HasOptions { get { return HasOptionJoin || HasOptionLeave; } }
    }
}