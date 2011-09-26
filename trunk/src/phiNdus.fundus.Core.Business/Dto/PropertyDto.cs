namespace phiNdus.fundus.Core.Business.Dto
{
    public class PropertyDto
    {
        public int Id { get; set; }

        public int Version { get; set; }

        public string Caption { get; set; }

        public PropertyDataType DataType { get; set; }

        public bool IsSystemProperty { get; set; }
    }
}