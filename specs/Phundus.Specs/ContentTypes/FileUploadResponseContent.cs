namespace Phundus.Specs.ContentTypes
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class FileUploadResponseContent
    {
        [JsonProperty("files")]
        public List<UploadedFile> Files { get; set; }
    }

    public class UploadedFile
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("isPreview")]
        public bool IsPreview { get; set; }
    }
}