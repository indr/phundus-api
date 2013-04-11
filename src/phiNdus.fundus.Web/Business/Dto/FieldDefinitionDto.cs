namespace phiNdus.fundus.Web.Business.Dto
{
    public class FieldDefinitionDto
    {
        public int Id { get; set; }
        
        public int Version { get; set; }

        public string Caption { get; set; }
        
        public FieldDataType DataType { get; set; }
        
        public bool IsSystem { get; set; }

        public int Position { get; set; }

        public bool IsDefault { get; set; }

        public bool IsColumn { get; set; }

        public bool IsAttachable { get; set; }
    }
}