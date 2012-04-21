namespace phiNdus.fundus.Core.Business.Dto
{
    public class ImageDto
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public bool IsPreview { get; set; }
        public long Length { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
    }
}