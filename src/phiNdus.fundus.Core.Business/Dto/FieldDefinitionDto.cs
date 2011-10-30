namespace phiNdus.fundus.Core.Business.Dto
{
    public class FieldDefinitionDto
    {
        public int Id { get; set; }

        public int Version { get; set; }

        public string Caption { get; set; }

        public FieldDataType DataType { get; set; }

        public bool IsSystemProperty { get; set; }
    }
}